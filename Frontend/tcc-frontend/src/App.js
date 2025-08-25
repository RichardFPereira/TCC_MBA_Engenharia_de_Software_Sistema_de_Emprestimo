import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Emprestimos from "./pages/Emprestimos";
import Admin from "./pages/Admin";
import Configuracoes from "./pages/Configuracoes";
import AutorizarEmprestimos from "./pages/AutorizarEmprestimos";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/emprestimos" element={<Emprestimos />} />
        <Route path="/admin" element={<Admin />} />
        <Route path="/configuracoes" element={<Configuracoes />} />
        <Route
          path="/AutorizarEmprestimos"
          element={<AutorizarEmprestimos />}
        />
      </Routes>
    </Router>
  );
}

export default App;
