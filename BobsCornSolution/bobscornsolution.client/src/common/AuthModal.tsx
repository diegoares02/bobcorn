import React, { useEffect, useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField } from '@mui/material';

type AuthMode = 'login' | 'register';

interface AuthModalProps {
    open: boolean;
    mode: AuthMode;
    onClose: () => void;
    onLogin: (email: string, password: string) => void;
    onRegister: (email: string, password: string, name: string, lastname: string) => void;
}

const AuthModal: React.FC<AuthModalProps> = ({
    open,
    mode,
    onClose,
    onLogin,
    onRegister,
}) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [name, setName] = useState('');
    const [lastname, setLastname] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (mode === 'login') {
            onLogin(email, password);
        } else {
            onRegister(email, password, name, lastname);
        }
        setEmail('');
        setPassword('');
        setName('');
        setLastname('');
    };

    if (!open) return null;

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>{mode === 'login' ? 'Login' : 'Register'}</DialogTitle>
            <form onSubmit={handleSubmit}>
                <DialogContent>
                    <TextField
                        label="Email"
                        type="email"
                        required
                        fullWidth
                        margin="normal"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                    />
                    <TextField
                        label="Password"
                        type="password"
                        required
                        fullWidth
                        margin="normal"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                    />
                    {mode === 'register' && (
                        <>
                            <TextField
                                label="Name"
                                type="text"
                                required
                                fullWidth
                                margin="normal"
                                value={name}
                                onChange={e => setName(e.target.value)}
                            />
                            <TextField
                                label="Lastname"
                                type="text"
                                required
                                fullWidth
                                margin="normal"
                                value={lastname}
                                onChange={e => setLastname(e.target.value)}
                            />
                        </>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button type="submit" variant="contained" color="primary">
                        {mode === 'login' ? 'Login' : 'Register'}
                    </Button>
                    <Button onClick={onClose} color="secondary">
                        Cancel
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};

export default AuthModal;