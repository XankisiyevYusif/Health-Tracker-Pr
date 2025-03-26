import React, { useEffect } from "react";
import { Routes, Route, useLocation, Router } from "react-router-dom";

import "./css/style.css";
import "./charts/ChartjsConfig";

// Import pages
import Dashboard from "./pages/Dashboard";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Profile from "./pages/Profile";
import CaloriesCaculator from "./pages/CaloriesCaculator"

function App() {
  const location = useLocation();

  useEffect(() => {
    document.querySelector("html").style.scrollBehavior = "auto";
    window.scroll({ top: 0 });
    document.querySelector("html").style.scrollBehavior = "";
  }, [location.pathname]);

  return (
    <Routes>
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />

      <Route path="/dashboard" element={<Dashboard />} />
      <Route path="/profile" element={<Profile />} />
      <Route path="/calculator" element={<CaloriesCaculator/>}/>
    </Routes>
  );
}

export default App;
