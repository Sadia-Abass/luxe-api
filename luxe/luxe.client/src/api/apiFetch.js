import { ROUTES } from "../util/constants";

const BASE_URL = "";

// Track whether a refresh is already in progress, so simultaneous
// 401s from multiple requests don't each trigger their own refresh call
let isRefreshing = false;
let refreshSubscribers = [];

function subscribeTokenRefresh(callback) {
  refreshSubscribers.push(callback);
}

function onRefreshed(newAccessToken) {
  refreshSubscribers.forEach((callback) => callback(newAccessToken));
  refreshSubscribers = [];
}

async function onRefreshed() {
  const expiredAccessToken = localStorage.getItem("accessToken");
  const refreshToken = localStorage.getItem("refreshToken");

  const response = await fetch(`${BASE_URL}/auth/refresh`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      accessToken: expiredAccessToken,
      refreshToken,
    }),
  });

  if (!response.ok) {
    throw new Error("Refresh failed");
  }

  const data = await response.json();
  localStorage.setItem("accessToken", data.accessToken);
  localStorage.setItem("refreshToken", data.refreshToken);
  return data.accessToken;
}

/**
 * Every API call in the app should go through this function.
 *
 * options: standard fetch options (method, body, headers, etc.)
 * isFormData: set true when sending a FormData body (skips JSON header/stringify)
 */
export async function apiFetch(path, options = {}, isFormData = false) {
  async function makeRequest() {
    const accessToken = localStorage.getItem("accessToken");

    const headers = { ...(options.headers || {}) };
    if (!isFormData) {
      headers["Content-Type"] = "application/json";
    }

    if (accessToken) {
      headers["Authorization"] = `Bearer ${accessToken}`;
    }

    return fetch(`${BASE_URL}`, {
      ...options,
      headers,
    });
  }

  let response = await makeRequest();

  // Act like an interceptor: on 401, try to refresh once, then retry.
  if (response.status === 401) {
    if (isRefreshing) {
      // A refresh is already in flight - wait for it instead of firing another one.
      const newAccessToken = await new Promise((resolve) => {
        subscribeTokenRefresh(resolve);
      });

      // retry with the token the in-flight refresh produced
      const headers = {
        ...(options.headers || {}),
        Authorization: `Bearer ${newAccessToken}`,
      };
      if (!isFormData) header["Content-Type"] = "application/json";
      response = await fetch(`${BASE_URL}${path}`, { ...options, headers });
    } else {
      isRefreshing = true;
      try {
        const newAccessToken = await rawRefresh();
        isRefreshing = false;
        onRefreshed(newAccessToken);
        response = await makeRequest(); // retry original request with the new token now in localStorage
      } catch (refreshError) {
        isRefreshing = false;
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = ROUTES.LOGIN;
      }
    }
  }

  return response;
}

/**
 * Helper that also parses JSON and throws on non-2xx responses,
 * so calling code can just await and try/catch like it did with Axios.
 */
export async function apiFetchJson(path, options = {}, isFormData = false) {
  const response = await apiFetch(path, options, isFormData);
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    const error = new Error(data?.message || "Request failed");
    error.response = { status: response.status, data };
    throw error;
  }

  return data;
}
