import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function CadastroUsuario() {
  const [formData, setFormData] = useState({
    nome: "",
    cpf: "",
    dataNascimento: "",
    email: "",
    senha: "",
  });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // Cadastro
      await axios.post(
        "http://127.0.0.1:5003/api/Usuarios/cadastro",
        formData,
        {
          headers: {
            Authorization: localStorage.getItem("token")
              ? `Bearer ${localStorage.getItem("token")}`
              : undefined,
          },
        }
      );

      // Login automático
      const loginResponse = await axios.post(
        "http://127.0.0.1:5003/api/Usuarios/login",
        {
          email: formData.email,
          senha: formData.senha,
        }
      );
      const { token } = loginResponse.data;
      localStorage.setItem("token", token); // Armazena o token

      setMessage("Cadastro realizado com sucesso! Redirecionando...");
      // Redireciona para a página inicial após 1 segundo
      setTimeout(() => navigate("/inicio"), 1000);
    } catch (error) {
      console.error("Erro ao cadastrar ou logar:", error);
      setMessage(error.response?.data || "Erro ao cadastrar ou logar usuário.");
    }
  };

  return (
    <div className="cadastro-container">
      <h2>Cadastro de Usuário</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Nome:</label>
          <input
            type="text"
            name="nome"
            value={formData.nome}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>CPF (11 dígitos):</label>
          <input
            type="text"
            name="cpf"
            value={formData.cpf}
            onChange={handleChange}
            required
            pattern="\d{11}"
          />
        </div>
        <div>
          <label>Data de Nascimento:</label>
          <input
            type="date"
            name="dataNascimento"
            value={formData.dataNascimento}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Senha:</label>
          <input
            type="password"
            name="senha"
            value={formData.senha}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit">Cadastrar</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default CadastroUsuario;
