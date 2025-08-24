import React, { useState, useEffect } from "react";
import axios from "axios";
import "../styles.css";

function Emprestimos() {
  const [emprestimos, setEmprestimos] = useState([]);
  const [valor, setValor] = useState("");
  const [numeroParcelas, setNumeroParcelas] = useState("");
  const [temEmprestimoEmAndamento, setTemEmprestimoEmAndamento] =
    useState(false);

  useEffect(() => {
    const fetchEmprestimos = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get(
          "http://127.0.0.1:5003/api/Emprestimos/usuario/2",
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        console.log("Resposta da API:", response.data);
        const data = response.data;
        setEmprestimos(data);

        setTemEmprestimoEmAndamento(
          data.some(
            (emp) => emp.status === "Pendente" || emp.status === "Em Andamento"
          )
        );
      } catch (error) {
        console.error(
          "Erro ao buscar empréstimos:",
          error.response ? error.response.data : error.message
        );
      }
    };
    fetchEmprestimos();
  }, []);

  const handleCriarEmprestimo = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
    try {
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
    } catch (error) {
      console.error(
        "Erro ao criar empréstimo:",
        error.response ? error.response.data : error.message
      );
      alert(
        "Erro: Não é possível criar um novo empréstimo enquanto houver um em andamento."
      );
    }
  };

  return (
    <div className="emprestimos-container">
      <h2>Meus Empréstimos</h2>
      <ul className="emprestimos-list">
        {emprestimos.length > 0 ? (
          emprestimos.map((emp) => (
            <li key={emp.id}>
              ID: {emp.id} - Valor: {emp.valor} - Valor Total: {emp.valorTotal}{" "}
              - Parcelas: {emp.numeroParcelas} - Status: {emp.status}
            </li>
          ))
        ) : (
          <li>Nenhum empréstimo em andamento.</li>
        )}
      </ul>
      {temEmprestimoEmAndamento && (
        <p style={{ color: "red" }}>
          Não é possível solicitar um novo empréstimo enquanto houver um em
          andamento.
        </p>
      )}
      <form className="emprestimos-form" onSubmit={handleCriarEmprestimo}>
        <input
          type="number"
          value={valor}
          onChange={(e) => setValor(e.target.value)}
          placeholder="Valor"
          disabled={temEmprestimoEmAndamento}
        />
        <input
          type="number"
          value={numeroParcelas}
          onChange={(e) => setNumeroParcelas(e.target.value)}
          placeholder="Parcelas"
          disabled={temEmprestimoEmAndamento}
        />
        <button type="submit" disabled={temEmprestimoEmAndamento}>
          Criar Empréstimo
        </button>
      </form>
    </div>
  );
}

export default Emprestimos;
