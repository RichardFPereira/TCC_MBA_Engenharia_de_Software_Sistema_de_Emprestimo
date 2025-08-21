import React, { useState, useEffect } from "react";
import axios from "axios";
import "../styles.css";

function Configuracoes() {
  const [configuracoes, setConfiguracoes] = useState({
    id: 0,
    taxaJuros: 0,
    minParcelas: 0,
    maxParcelas: 0,
    percentualReserva: 0,
  });
  const [originalConfiguracoes, setOriginalConfiguracoes] = useState({});
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    const fetchConfiguracoes = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get(
          "http://127.0.0.1:5003/api/Configuracoes",
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        const data = response.data;
        setConfiguracoes(data);
        setOriginalConfiguracoes({ ...data });
      } catch (error) {
        console.error(
          "Erro ao buscar configurações:",
          error.response ? error.response.data : error.message
        );
      }
    };
    fetchConfiguracoes();
  }, []);

  const handleEdit = () => {
    setEditMode(true);
  };

  const handleSave = async () => {
    const token = localStorage.getItem("token");
    try {
      const dataToSend = {
        taxaJuros: configuracoes.taxaJuros,
        minParcelas: configuracoes.minParcelas,
        maxParcelas: configuracoes.maxParcelas,
        percentualReserva: configuracoes.percentualReserva,
      };
      await axios.post("http://127.0.0.1:5003/api/Configuracoes", dataToSend, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setEditMode(false);
      alert("Configurações salvas com sucesso!");

      const response = await axios.get(
        "http://127.0.0.1:5003/api/Configuracoes",
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setConfiguracoes(response.data);
      setOriginalConfiguracoes({ ...response.data });
    } catch (error) {
      console.error(
        "Erro ao salvar configurações:",
        error.response ? error.response.data : error.message
      );
      alert("Erro ao salvar configurações");
    }
  };

  const handleCancel = () => {
    setConfiguracoes({ ...originalConfiguracoes });
    setEditMode(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setConfiguracoes((prev) => ({
      ...prev,
      [name]: parseFloat(value) || value,
    }));
  };

  return (
    <div className="emprestimos-container">
      <h2>Configurações de Empréstimo</h2>
      <div>
        <label>
          Taxa de Juros (%):
          {editMode ? (
            <input
              type="number"
              name="taxaJuros"
              value={configuracoes.taxaJuros}
              onChange={handleChange}
              step="0.01"
            />
          ) : (
            <span> {configuracoes.taxaJuros}%</span>
          )}
        </label>
        <br />
        <label>
          Mínimo de Parcelas:
          {editMode ? (
            <input
              type="number"
              name="minParcelas"
              value={configuracoes.minParcelas}
              onChange={handleChange}
            />
          ) : (
            <span> {configuracoes.minParcelas}</span>
          )}
        </label>
        <br />
        <label>
          Máximo de Parcelas:
          {editMode ? (
            <input
              type="number"
              name="maxParcelas"
              value={configuracoes.maxParcelas}
              onChange={handleChange}
            />
          ) : (
            <span> {configuracoes.maxParcelas}</span>
          )}
        </label>
        <br />
        <label>
          Percentual de Reserva (%):
          {editMode ? (
            <input
              type="number"
              name="percentualReserva"
              value={configuracoes.percentualReserva}
              onChange={handleChange}
              step="0.1"
            />
          ) : (
            <span> {configuracoes.percentualReserva}%</span>
          )}
        </label>
        <br />
        <label>
          Data da Última Alteração:{" "}
          <span>
            {new Date(configuracoes.dataCadastro).toLocaleString("pt-BR", {
              day: "2-digit",
              month: "2-digit",
              year: "numeric",
              hour: "2-digit",
              minute: "2-digit",
              second: "2-digit",
            })}
          </span>
        </label>
        <br />
        {editMode && (
          <>
            <button onClick={handleSave}>Salvar</button>
            <button onClick={handleCancel} style={{ marginLeft: "10px" }}>
              Cancelar
            </button>
          </>
        )}
        {!editMode && <button onClick={handleEdit}>Editar</button>}
      </div>
    </div>
  );
}

export default Configuracoes;
