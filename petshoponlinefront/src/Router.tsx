import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import LoginPage from './pages/LoginPage';
import CadastroPage from './pages/RegisterPage';

export default function Router() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/cadastro" element={<CadastroPage />} />
                <Route path="/login" element={<LoginPage />} />
                {/* Outras rotas futuras */}
            </Routes>
        </BrowserRouter>
    );
}