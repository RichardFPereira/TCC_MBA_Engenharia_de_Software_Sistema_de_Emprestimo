import React, { useState, useEffect } from "react";
import axios from "axios";

function AutorizarEmprestimos() {
  const [emprestimos, setEmprestimos] = useState([]);
  const [selecoes, setSelecoes] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchEmprestimos = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get(
          "http://127.0.0.1:5003/api/Emprestimos/pendentes",
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        setEmprestimos(response.data);

        const initialSelecoes = response.data.reduce(
          (acc, emp) => ({
            ...acc,
            [emp.id]: { autorizar: false, rejeitar: false },
          }),
          {}
        );
        setSelecoes(initialSelecoes);
      } catch (error) {
        console.error("Erro ao buscar empréstimos:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchEmprestimos();
  }, []);

  const handleCheckboxChange = (id, tipo) => {
    setSelecoes((prev) => {
      const newSelecoes = { ...prev[id], [tipo]: !prev[id][tipo] };
      if (tipo === "autorizar" && newSelecoes.autorizar)
        newSelecoes.rejeitar = false;
      if (tipo === "rejeitar" && newSelecoes.rejeitar)
        newSelecoes.autorizar = false;
      return { ...prev, [id]: newSelecoes };
    });
  };

  const handleSalvar = async () => {
    if (!Object.values(selecoes).some((s) => s.autorizar || s.rejeitar)) {
      alert(
        "Selecione pelo menos uma ação (autorizar ou rejeitar) para prosseguir."
      );
      return;
    }

    if (!window.confirm("Deseja realmente prosseguir com a operação?")) {
      return;
    }

    const token = localStorage.getItem("token");
    const batchData = Object.entries(selecoes)
      .filter(([, s]) => s.autorizar || s.rejeitar)
      .map(([id, s]) => ({
        id: parseInt(id),
        autorizar: s.autorizar,
      }));

    try {
      await axios.put(
        "http://127.0.0.1:5003/api/Emprestimos/autorizar-batch",
        batchData,
        { headers: { Authorization: `Bearer ${token}` } }
      );

      window.location.reload();
    } catch (error) {
      console.error("Erro ao processar autorizações:", error);
      alert("Erro ao salvar as alterações. Verifique o console para detalhes.");
    }
  };

  if (loading) return <p>Carregando...</p>;

  return (
    <div className="autorizar-container">
      <h2>Autorizar Empréstimos Pendentes</h2>
      <table className="emprestimos-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Email</th>
            <th>Valor</th>
            <th>Parcelas</th>
            <th>Data</th>
            <th>Autorizar</th>
            <th>Rejeitar</th>
          </tr>
        </thead>
        <tbody>
          {emprestimos.map((emp) => (
            <tr key={emp.id}>
              <td>{emp.id}</td>
              <td>{emp.nomeUsuario}</td>
              <td>{emp.email}</td>
              <td>{emp.valor}</td>
              <td>{emp.numeroParcelas}</td>
              <td>{new Date(emp.dataEmprestimo).toLocaleDateString()}</td>
              <td>
                <input
                  type="checkbox"
                  checked={selecoes[emp.id]?.autorizar || false}
                  onChange={() => handleCheckboxChange(emp.id, "autorizar")}
                />
              </td>
              <td>
                <input
                  type="checkbox"
                  checked={selecoes[emp.id]?.rejeitar || false}
                  onChange={() => handleCheckboxChange(emp.id, "rejeitar")}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <button onClick={handleSalvar}>Salvar Alterações</button>
    </div>
  );
}

export default AutorizarEmprestimos;
