import { useState } from "react";
import addressService from "../services/AddressService";
import { useNavigate } from "react-router-dom";
import HomeMenu from "../components/menu/Menu";

export default function AddAddressPage() {
    const [form, setForm] = useState({
        logradouro: "",
        numero: "",
        bairro: "",
        cidade: "",
        estado: "",
        cep: ""
    });
    const [msg, setMsg] = useState("");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        setForm({ ...form, [e.target.name]: e.target.value });
    }

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setMsg("");
        setLoading(true);
        try {
            await addressService.criarEndereco(form);
            setMsg("Endereço cadastrado com sucesso!");
            setTimeout(() => navigate("/order"), 1200);
        } catch {
            setMsg("Erro ao cadastrar endereço");
        }
        setLoading(false);
    }

    return (
        <>
            <header style={{
                width: '100vw',
                background: '#00a2ffff',
                boxShadow: '0 2px 8px rgba(46,139,87,0.12)',
                padding: '0.5rem 0',
                position: 'sticky',
                top: 0,
                zIndex: 10
            }}>
                <HomeMenu horizontal />
            </header>
            <div style={{
                minHeight: '100vh',
                background: '#f7f7f7',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontFamily: 'Segoe UI, Arial, sans-serif'
            }}>
                <form onSubmit={handleSubmit} style={{
                    background: '#fff',
                    borderRadius: '20px',
                    boxShadow: '0 8px 32px rgba(33,150,243,0.13)',
                    padding: '2.5rem 3.5rem',
                    maxWidth: 500,
                    width: '100%'
                }}>
                    <h2 style={{ color: '#2196f3', fontWeight: 800, fontSize: '2rem', marginBottom: '1.5rem' }}>Novo Endereço</h2>
                    {msg && <div style={{ color: msg.startsWith("Erro") ? "#d32f2f" : "#267d4a", marginBottom: 12 }}>{msg}</div>}
                    <input name="logradouro" placeholder="Logradouro" value={form.logradouro} onChange={handleChange} required style={{ marginBottom: 10, width: "100%", padding: 8 }} />
                    <input name="numero" placeholder="Número" value={form.numero} onChange={handleChange} required style={{ marginBottom: 10, width: "100%", padding: 8 }} />
                    <input name="bairro" placeholder="Bairro" value={form.bairro} onChange={handleChange} required style={{ marginBottom: 10, width: "100%", padding: 8 }} />
                    <input name="cidade" placeholder="Cidade" value={form.cidade} onChange={handleChange} required style={{ marginBottom: 10, width: "100%", padding: 8 }} />
                    <input name="estado" placeholder="Estado" value={form.estado} onChange={handleChange} required style={{ marginBottom: 10, width: "100%", padding: 8 }} />
                    <input name="cep" placeholder="CEP" value={form.cep} onChange={handleChange} required style={{ marginBottom: 18, width: "100%", padding: 8 }} />
                    <button
                        type="submit"
                        disabled={loading}
                        style={{
                            background: '#2196f3',
                            color: '#fff',
                            border: 'none',
                            borderRadius: 8,
                            padding: '0.7rem 2.2rem',
                            fontWeight: 700,
                            fontSize: '1.1rem',
                            cursor: loading ? 'not-allowed' : 'pointer'
                        }}
                    >
                        {loading ? "Salvando..." : "Salvar Endereço"}
                    </button>
                </form>
            </div>
        </>
    );
}