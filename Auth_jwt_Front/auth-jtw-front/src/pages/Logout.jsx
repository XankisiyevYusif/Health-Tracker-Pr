import { useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Logout({ setAuth }) {
  const navigate = useNavigate();

  useEffect(() => {
    const handleLogout = async () => {
      try {
        await axios.post("https://localhost:7094/api/UserAuth/Logout");
        localStorage.removeItem("token");
        delete axios.defaults.headers.common["Authorization"];
        setAuth(false);
        navigate("/login");
      } catch (err) {
        console.error("Logout failed", err);
      }
    };
    handleLogout();
  }, [navigate, setAuth]);

  return <h2>Logging out...</h2>;
}

export default Logout;