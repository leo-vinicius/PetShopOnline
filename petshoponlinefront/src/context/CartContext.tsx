import React, { createContext, useContext, useReducer, ReactNode } from 'react';

// Tipos
export interface CartItem {
    id: string;
    name: string;
    price: number;
    quantity: number;
    imageUrl?: string;
}

interface CartState {
    items: CartItem[];
}

type CartAction =
    | { type: 'ADD_ITEM'; payload: CartItem }
    | { type: 'REMOVE_ITEM'; payload: string }
    | { type: 'CLEAR_CART' }
    | { type: 'UPDATE_QUANTITY'; payload: { id: string; quantity: number } };

// Estado inicial
const initialState: CartState = {
    items: [],
};

// Reducer
function cartReducer(state: CartState, action: CartAction): CartState {
    switch (action.type) {
        case 'ADD_ITEM': {
            const itemExists = state.items.find(item => item.id === action.payload.id);

            if (itemExists) {
                return {
                    items: state.items.map(item =>
                        item.id === action.payload.id
                            ? { ...item, quantity: item.quantity + action.payload.quantity }
                            : item
                    ),
                };
            } else {
                return {
                    items: [...state.items, action.payload],
                };
            }
        }

        case 'REMOVE_ITEM': {
            return {
                items: state.items.filter(item => item.id !== action.payload),
            };
        }

        case 'CLEAR_CART': {
            return {
                items: [],
            };
        }

        case 'UPDATE_QUANTITY': {
            return {
                items: state.items.map(item =>
                    item.id === action.payload.id
                        ? { ...item, quantity: action.payload.quantity }
                        : item
                ),
            };
        }

        default:
            return state;
    }
}

// Contexto
const CartContext = createContext<{
    state: CartState;
    addItem: (item: CartItem) => void;
    removeItem: (id: string) => void;
    clearCart: () => void;
    updateQuantity: (id: string, quantity: number) => void;
}>({
    state: initialState,
    addItem: () => { },
    removeItem: () => { },
    clearCart: () => { },
    updateQuantity: () => { },
});

// Provedor
export const CartProvider = ({ children }: { children: ReactNode }) => {
    const [state, dispatch] = useReducer(cartReducer, initialState);

    const addItem = (item: CartItem) => dispatch({ type: 'ADD_ITEM', payload: item });
    const removeItem = (id: string) => dispatch({ type: 'REMOVE_ITEM', payload: id });
    const clearCart = () => dispatch({ type: 'CLEAR_CART' });
    const updateQuantity = (id: string, quantity: number) =>
        dispatch({ type: 'UPDATE_QUANTITY', payload: { id, quantity } });

    return (
        <CartContext.Provider value={{ state, addItem, removeItem, clearCart, updateQuantity }}>
            {children}
        </CartContext.Provider>
    );
};

// Hook de uso
export const useCart = () => useContext(CartContext);
