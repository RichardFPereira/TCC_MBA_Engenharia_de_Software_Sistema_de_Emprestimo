import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./components/Login";
import Emprestimos from "./components/Emprestimos";

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
