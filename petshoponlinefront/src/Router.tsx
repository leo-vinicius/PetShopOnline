import { BrowserRouter, Route, Routes } from 'react-router-dom';
import AdminPage from './pages/AdminPage';
import CartPage from './pages/CartPage';
import Home from './pages/Home';
import LoginPage from './pages/LoginPage';
import OrderPage from './pages/OrderPage';
import ProductDetailPage from './pages/ProductDetailsPage';
import ProductsPage from './pages/ProductsPage';
import ProfilePage from './pages/ProfilePage';
import CadastroPage from './pages/RegisterPage';
import AddAddressPage from './pages/NewAddressPage';
import OrderSuccessPage from './pages/OrderSuccessPage';


export default function Router() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/cadastro" element={<CadastroPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/perfil" element={<ProfilePage />} />
                <Route path="/admin" element={<AdminPage />} />
                <Route path="/carrinho" element={<CartPage />} />
                <Route path="/order" element={<OrderPage />} />
                <Route path="/produtos" element={<ProductsPage />} />
                <Route path="/produtos/:id" element={<ProductDetailPage />} />
                <Route path="/order" element={<OrderPage />} />
                <Route path="/add-address" element={<AddAddressPage />} />
                <Route path="/order/success" element={<OrderSuccessPage />} />
            </Routes>
        </BrowserRouter>
    );
}