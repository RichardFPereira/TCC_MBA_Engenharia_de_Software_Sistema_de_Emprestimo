import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Inicio() {
  const [dados, setDados] = useState({ reserva: 0, emprestimos: [] });
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDados = async () => {
      const token = localStorage.getItem("token");
      if (!token) {
        navigate("/");
        return;
      }
      const response = await axios.get(
        "http://127.0.0.1:5003/api/Emprestimos/usuario/{id}",
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setDados(response.data);
    };
    fetchDados();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  return (
    <div className="inicio-container">
      <h2>Dashboard - Participante</h2>
      <p>Reserva: R$ {dados.reserva}</p>
      <p>
        Empréstimos Ativos:{" "}
        {dados.emprestimos.filter((e) => e.status === "Em Andamento").length}
      </p>
      <button onClick={handleLogout}>Sair</button>
    </div>
  );
}

export default Inicio;
