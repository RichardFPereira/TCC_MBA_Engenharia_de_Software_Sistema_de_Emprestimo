import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./pages/Login";
import Emprestimos from "./pages/Emprestimos";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/emprestimos" element={<Emprestimos />} />
      </Routes>
    </Router>
  );
}

export default App;
