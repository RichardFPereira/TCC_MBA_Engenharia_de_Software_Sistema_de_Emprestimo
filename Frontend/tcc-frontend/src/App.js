import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Emprestimos from "./pages/Emprestimos";
import Admin from "./pages/Admin";
import Configuracoes from "./pages/Configuracoes";
import AutorizarEmprestimos from "./pages/AutorizarEmprestimos";
import CadastroUsuario from "./pages/CadastroUsuario";
import Inicio from "./pages/Inicio";

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
        <Route path="/cadastro" element={<CadastroUsuario />} />
        <Route path="/inicio" element={<Inicio />} />
      </Routes>
    </Router>
  );
}

export default App;
