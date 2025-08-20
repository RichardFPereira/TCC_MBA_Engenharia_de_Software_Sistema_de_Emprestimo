import React, { useState, useEffect } from "react";
import axios from "axios";

function Emprestimos() {
  const [emprestimos, setEmprestimos] = useState([]);
  const [valor, setValor] = useState("");
  const [numeroParcelas, setNumeroParcelas] = useState("");

  useEffect(() => {
    const fetchEmprestimos = async () => {
      const token = localStorage.getItem("token");
      const response = await axios.get(
        "http://127.0.0.1:5003/api/Emprestimos/usuario/2",
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setEmprestimos(response.data);
    };
    fetchEmprestimos();
  }, []);

  const handleCriarEmprestimo = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
    await axios.post(
      "http://127.0.0.1:5003/api/Emprestimos",
      {
        valor: parseFloat(valor),
        numeroParcelas: parseInt(numeroParcelas),
      },
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
    alert("Empréstimo criado!");
    window.location.reload();
  };

  return (
    <div>
      <h2>Meus Empréstimos</h2>
      <ul>
        {emprestimos.map((emp) => (
          <li key={emp.Id}>
            {emp.Valor} - {emp.Status}
          </li>
        ))}
      </ul>
      <form onSubmit={handleCriarEmprestimo}>
        <input
          type="number"
          value={valor}
          onChange={(e) => setValor(e.target.value)}
          placeholder="Valor"
        />
        <input
          type="number"
          value={numeroParcelas}
          onChange={(e) => setNumeroParcelas(e.target.value)}
          placeholder="Parcelas"
        />
        <button type="submit">Criar Empréstimo</button>
      </form>
    </div>
  );
}

export default Emprestimos;
