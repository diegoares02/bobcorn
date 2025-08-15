import { useEffect, useState } from "react";
import Navbar from "./common/Navbar";
import './App.css'
import AuthModal from "./common/AuthModal";
import { useDispatch, useSelector } from "react-redux";
import { login, register } from "./store/slices/userSlice"
import BuyButton from "./components/BuyButton";
import { API_URL } from "./constants/urls";

type AuthMode = 'login' | 'register';

function App() {
    const [mode, setMode] = useState<AuthMode>('login')
    const [open, setOpen] = useState(false)
    const [isLogged, setIsLogged] = useState(false);

    const [data, setData] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const dispatch = useDispatch<any>();
    const userState = useSelector<any>(state => state)

    const fetchData = async (parseToken: string) => {
                try {
                    setLoading(true);
                    const response = await fetch(`${API_URL}/api/product/available`, {
                        method: 'GET', 
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${parseToken}`,
                        }
                    });
                    if (!response.ok) {
                        throw new Error(`HTTP error! status: ${response.status}`);
                    }
                    const result = await response.json();
                    setData(result);
                } catch (e: any) {
                    setError(e.message);
                } finally {
                    setLoading(false);
                }
            };

    useEffect(() => {
        const token = localStorage.getItem('token');
        const parseToken = token && JSON.parse(token).token;
        if (parseToken) {            
            fetchData(parseToken);
        }
    }, []);

    useEffect(() => {
        if (userState && userState.user?.data.data !== null) {
            setIsLogged(true);
        }
        else {
            setIsLogged(false)
        }
    })

    const handleReloadAvailability = () => {
        const token = localStorage.getItem('token');
        const parseToken = token && JSON.parse(token).token;
        if (parseToken) {            
            fetchData(parseToken);
        }
    }

    const handleClose = () => {
        setOpen(false);
        setMode('login');
    }

    const handleOpenModal = (authMode: AuthMode) => {
        setMode(authMode);
        setOpen(true);
    }

    const handleLogin = (email: string, password: string) => {
        dispatch(login({ email, password }));
        setOpen(false);
        setMode('login');
    }
    const handleRegister = (email: string, password: string, name: string, lastname: string) => {
        dispatch(register({ email, password, name, lastname }))
    }

    const cornInfo =
        <>
            <img src="https://media.istockphoto.com/id/1521680045/vector/simple-corn-clipart-vector-illustration-isolated-on-white-background-cute-corn-or-corncob.jpg?s=612x612&w=0&k=20&c=b2A2gpqDfcyYfREw4sDbbLjPrgyKzpNwAFXNLAawVBM=" width={120} height={120} />
            <br />
            <h2>Available</h2>
            <h3>{data.data.quantity}</h3>
            <br />
            <BuyButton onReloadAvailability={handleReloadAvailability}/>
        </>;

    return (
        <>
            <Navbar onOpenModal={handleOpenModal} />
            <br />
            {isLogged && cornInfo}
            <AuthModal open={open} mode={mode} onClose={handleClose} onLogin={handleLogin} onRegister={handleRegister} />
        </>
    );
}

export default App;