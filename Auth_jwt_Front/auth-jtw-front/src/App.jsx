import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Logout from "./pages/Logout";
import { useState, useEffect } from "react";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem("token"));

  useEffect(() => {
    setIsAuthenticated(!!localStorage.getItem("token"));
  }, []);

  return (
    <Router>
      <div style={{ display: "flex", flexDirection: "column", alignItems: "center", marginTop: "50px" }}>
        <h1>Auth System</h1>
        <Routes>
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login setAuth={setIsAuthenticated} />} />
          <Route path="/logout" element={<Logout setAuth={setIsAuthenticated} />} />
          <Route path="/" element={isAuthenticated ? <Navigate to="/logout" /> : <Navigate to="/login" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;