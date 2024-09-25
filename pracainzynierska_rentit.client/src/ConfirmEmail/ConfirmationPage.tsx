import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './ConfirmationPage.css';

const ConfirmationPage: React.FC = () => {
    const [countdown, setCountdown] = useState(5);
    const navigate = useNavigate();

    useEffect(() => {
        const interval = setInterval(() => {
            setCountdown(prevCountdown => prevCountdown - 1);
        }, 1000);

        if (countdown === 0) {
            window.location.href = "/";
        }

        return () => clearInterval(interval);
    }, [countdown, navigate]);

    const handleSkip = () => {
        window.location.href = "/";
    };

    return (
        <div className="confirmation-container">
            <h1 className="h1-confirm">Email został potwierdzony!</h1>
            <p className="p-confirm">Automatyczne przeniesienie za {countdown}...</p>
            <button className="skip-button" onClick={handleSkip}>Skip</button>
        </div>
    );
};

export default ConfirmationPage;