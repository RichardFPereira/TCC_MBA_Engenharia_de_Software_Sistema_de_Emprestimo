import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Emprestimos from "./pages/Emprestimos";
import Admin from "./pages/Admin";
import Configuracoes from "./pages/Configuracoes";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/emprestimos" element={<Emprestimos />} />
        <Route path="/admin" element={<Admin />} />
        <Route path="/configuracoes" element={<Configuracoes />} />
      </Routes>
    </Router>
  );
}

export default App;
