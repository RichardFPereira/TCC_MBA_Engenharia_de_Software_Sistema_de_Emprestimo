import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../styles.css";

function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://127.0.0.1:5003/api/Usuarios/login",
        {
          email: email,
          senha: senha,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      localStorage.setItem("token", response.data.token);
      setMessage("Login realizado com sucesso! Redirecionando...");

      setTimeout(() => navigate("/inicio"), 1000);
    } catch (error) {
      console.error(
        "Erro no login:",
        error.response ? error.response.data : error.message
      );
      setMessage("");
      alert("Credenciais inválidas ou erro na conexão");
    }
  };

  return (
    <div className="container">
      <div className="form">
        <form onSubmit={handleLogin}>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Email"
            required
          />
          <input
            type="password"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            placeholder="Senha"
            required
          />
          <button type="submit">Login</button>
        </form>
        {message && <p>{message}</p>}
        <p>
          Ainda não tem conta? <a href="/cadastro">Cadastre-se</a>
        </p>
      </div>
    </div>
  );
}

export default Login;
