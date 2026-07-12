import { apiFetchJson } from "./apiFetch";

function buildQuery(params) {
  return new URLSearchParams(params).toString();
}

export const login = (email, password) =>
  apiFetchJson("/authentication/login", {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });

export const register = (formData) =>
  apiFetchJson(
    "/authentication/register",
    {
      method: "POST",
      body: formData,
    },
    true,
  ); // isFormData = true, skips Content-Type/JSON.stringify

export const logout = (refreshToken) =>
  apiFetchJson("/authentication/revoke", {
    method: "POST",
    body: JSON.stringify({ refreshToken }),
  });

export const forgotPassword = (email) =>
  apiFetchJson("/authentication/forgot-password", {
    method: "POST",
    body: JSON.stringify({ email }),
  });

export const resetPassword = (userId, token, newPassword) =>
  apiFetchJson("/authentication/reset-password", {
    method: "POST",
    body: JSON.stringify({ userId, token, newPassword }),
  });

export const changePassword = (currentPassword, newPassword) =>
  apiFetchJson("/authentication/change-password", {
    method: "POST",
    body: JSON.stringify({ currentPassword, newPassword }),
  });

export const confirmEmail = (userId, token) =>
  apiFetchJson(
    `/authentication/confirm-email?userId=${encodeURIComponent(userId)}&token=${encodeURIComponent(token)}`,
    {
      method: "GET",
    },
  );
