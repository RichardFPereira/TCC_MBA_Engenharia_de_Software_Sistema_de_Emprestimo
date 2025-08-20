import React, { useState, useEffect } from "react";
import axios from "axios";
import "../styles.css";

function Admin() {
  const [parcelas, setParcelas] = useState([]);
  const [emprestimoId, setEmprestimoId] = useState("");
  const [valorParcela, setValorParcela] = useState("");

  useEffect(() => {
    const fetchParcelas = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get("http://127.0.0.1:5003/api/Parcelas", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setParcelas(response.data);
      } catch (error) {
        console.error(
          "Erro ao buscar parcelas:",
          error.response ? error.response.data : error.message
        );
      }
    };
    fetchParcelas();
  }, []);

  const handleCriarParcela = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
    try {
      await axios.post(
        "http://127.0.0.1:5003/api/Parcelas",
        {
          emprestimoId: parseInt(emprestimoId),
          valor: parseFloat(valorParcela),
        },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      alert("Parcela criada!");
      window.location.reload();
    } catch (error) {
      console.error(
        "Erro ao criar parcela:",
        error.response ? error.response.data : error.message
      );
    }
  };

  return (
    <div className="emprestimos-container">
      <h2>Gerenciar Parcelas</h2>
      <ul className="emprestimos-list">
        {parcelas.map((parcela) => (
          <li key={parcela.Id}>
            Empréstimo {parcela.EmprestimoId} - Valor: {parcela.Valor} - Status:{" "}
            {parcela.Status}
          </li>
        ))}
      </ul>
      <form className="emprestimos-form" onSubmit={handleCriarParcela}>
        <input
          type="number"
          value={emprestimoId}
          onChange={(e) => setEmprestimoId(e.target.value)}
          placeholder="ID do Empréstimo"
        />
        <input
          type="number"
          value={valorParcela}
          onChange={(e) => setValorParcela(e.target.value)}
          placeholder="Valor da Parcela"
        />
        <button type="submit">Criar Parcela</button>
      </form>
    </div>
  );
}

export default Admin;
