import { ToastContainer } from "react-toastify";
import Header from "./components/layout/Header";
import Footer from "./components/layout/Footer";
import AppRouter from "./routes/AppRouter";

function App() {
  return (
    <div className="d-flex flex-column min-vh-100 bg-body">
      <Headers />
      <main>
        <AppRouter />
      </main>
      <Footer />
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick={false}
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
        transition={Bounce}
      />
    </div>
  );
}

export default App;
