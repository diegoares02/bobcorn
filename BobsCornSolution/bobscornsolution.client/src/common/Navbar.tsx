import React, { useEffect, useState } from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../store/slices/userSlice';

type AuthMode = 'login' | 'register';

interface NavbarProps {
    onOpenModal: (authMode: AuthMode) => void;
}

const Navbar: React.FC<NavbarProps> = ({ onOpenModal }) => {
    const [isLogged, setIsLogged] = useState(false);
    const userState = useSelector<any>((state) => state)
    const dispatch = useDispatch<any>();

    useEffect(() => {
        if (userState && userState.user.data && userState.user.data.data !== null) {
            setIsLogged(true);
        }
        else{
            setIsLogged(false);
        }
    });
    const handleLogout = () => {
        dispatch(logout());
    }
    return (
        <AppBar position="static" color="primary">
            <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Bob's Corn
                </Typography>
                <Box>
                    {!isLogged && <Button color="inherit" sx={{ mr: 1 }} onClick={() => onOpenModal('login')}>Login</Button>}
                    {!isLogged && <Button color="inherit" variant="outlined" onClick={() => onOpenModal('register')}>Register</Button>}
                    {isLogged && <Button color="inherit" sx={{ mr: 1 }} onClick={handleLogout}>Logout</Button>}
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;