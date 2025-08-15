import { Button } from "@mui/material";
import React from "react"
import { useDispatch, useSelector } from "react-redux";
import { buy } from "../store/slices/userSlice";

interface BuyButtonProps {
    onReloadAvailability: () => void
}

const BuyButton: React.FC<BuyButtonProps> = ({ onReloadAvailability }) => {
    const dispatch = useDispatch<any>()
    const userState = useSelector<any>(state => state);

    const handleBuy = () => {
        if (userState) {
            dispatch(buy({ userId: userState.user.data.data.user }));
            onReloadAvailability();
        }
    }
    return (<Button color="inherit" variant="outlined" onClick={handleBuy}>Buy</Button>);
}

export default BuyButton;