import React, { useEffect, useState } from 'react';
import styles from './settings.module.css';
import { FaUser, FaLock, FaEye, FaCreditCard, FaChevronDown, FaChevronUp, FaCheckCircle } from 'react-icons/fa';
import { Header } from "../Header/Header.tsx";
type BlockState = {
    daneOsobowe: boolean;
    logowanie: boolean;
    widoczność: boolean;
    danePłatnicze: boolean;
};

export default function Settings() {
    const [isEmailConfirmed, setIsEmailConfirmed] = useState(false);
    const [expandedBlocks, setExpandedBlocks] = useState<BlockState>({
        daneOsobowe: false,
        logowanie: false,
        widoczność: false,
        danePłatnicze: false
    });
    const [emailSent, setEmailSent] = useState(false); // New state for email verification status
    // State for the user inputs
    const [formData, setFormData] = useState({
        FirstName: '',
        LastName: '',
        Email: '',
        BirthDay: '',
        PhoneNumber: '',
        Password: '',
    });
    const [confirmPassword, setConfirmPassword] = useState('');
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        if (name === 'confirmPassword') {
            setConfirmPassword(value);
        } else {
            setFormData(prevState => ({
                ...prevState,
                [name]: value
            }));
        }
    };
    const handleToggle = (block: keyof BlockState) => {
        setExpandedBlocks(prevState => ({
            ...prevState,
            [block]: !prevState[block]
        }));
    };
    const handleEmailVerification = async () => {
        const response = await fetch("api/AspNetUsers/SendConfirmation", {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include'
        })
        if(response.ok){
            setEmailSent(true);
        }
    };
    const handleSaveChanges = async () => {
        const nonEmptyFields = Object.entries(formData).filter(([key, value]) => value !== '');

        if (nonEmptyFields.length > 0) {
            if (formData.Password !== confirmPassword) {
                alert('Passwords do not match!');
                return;
            }
            nonEmptyFields.forEach(([key, value]) => {
                console.log(`${key}: ${value}`);
            });
            const fieldsToSend = Object.fromEntries(nonEmptyFields);
            try {
                const response = await fetch("https://localhost:7214/api/AspNetUsers/Edit", {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(fieldsToSend),
                    credentials: 'include'
                });
                if (response.ok) {
                    console.log("Changes saved successfully.");
                    window.location.reload();
                } else {
                    console.error("Failed to save changes.");
                }
            } catch (error) {
                console.error("Error during saving:", error);
            }
        } else {
            console.log("No changes to save.");
        }
    };
    useEffect(() => {
        const checkEmailConfirmed = async () => {
            try {
                const response = await fetch("api/AspNetUsers/info", {
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                });
                if (response.ok) {
                    const data = await response.json();
                    setIsEmailConfirmed(data.emailConfirmed);
                } else {
                    window.location.href = "/";
                }
            } catch (error) {
                console.error("Error checking login status:", error);
            }
        };
        checkEmailConfirmed();
        
    }, []);
    return (
        <div className={styles.site}>
            {Header()}
            <div className={styles.container}>
                <div className={styles.verifyBlock}>
                    <FaCheckCircle className={styles.verification}/>
                    <div className={styles.textBlock}>
                        {isEmailConfirmed ? (
                            <>
                                <div className={styles.mainText}>Twój Email jest potwierdzony</div>
                                <div className={styles.subText}>Twoje konto zostało zweryfikowane i zatwierdzone. Możesz
                                    korzystać ze wszystkich możliwości na stronie.
                                </div>
                            </>
                        ) : (
                            <>
                                {emailSent ? (
                                    <>
                                        <div className={styles.mainText}>Email został wysłany</div>
                                        <div className={styles.subText}>Sprawdź swoją skrzynkę odbiorczą</div>
                                    </>
                                ) : (
                                    <>
                                        <div className={styles.mainText}>Zweryfikuj swój email</div>
                                        <div className={styles.subText}>Zweryfikuj swój email, aby móc korzystać w pełni
                                            z naszej strony
                                        </div>
                                    </>
                                )}
                            </>
                        )}
                    </div>
                    {!emailSent && !isEmailConfirmed && (
                        <button className={styles.verifyButton} onClick={handleEmailVerification}>
                            Zweryfikuj konto
                        </button>
                    )}
                </div>
                {/* Dane osobowe */}
                <div className={styles.block} onClick={() => handleToggle('daneOsobowe')}>
                    <FaUser className={styles.icon}/>
                    Dane osobowe
                    {expandedBlocks.daneOsobowe ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div className={`${styles.expandedContent} ${expandedBlocks.daneOsobowe ? styles.expanded : ''}`}>
                    <div className={styles.subBlock}>
                        <label>
                            Imie
                            <input
                                type="text"
                                name="FirstName"
                                value={formData.FirstName}
                                onChange={handleChange}
                                className={styles.input}
                                placeholder="test"
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Nazwisko
                            <input
                                type="text"
                                name="LastName"
                                value={formData.LastName}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Email
                            <input
                                type="email"
                                name="Email"
                                value={formData.Email}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Data urodzenia
                            <input
                                type="date"
                                name="BirthDay"
                                value={formData.BirthDay}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Telefon komórkowy
                            <input
                                type="tel"
                                name="PhoneNumber"
                                value={formData.PhoneNumber}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                </div>

                {/* Logowanie */}
                <div className={styles.block} onClick={() => handleToggle('logowanie')}>
                    <FaLock className={styles.icon}/>
                    Logowanie
                    {expandedBlocks.logowanie ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div className={`${styles.expandedContent} ${expandedBlocks.logowanie ? styles.expanded : ''}`}>
                    <div className={styles.subBlock}>
                        <label>
                            New Password
                            <input
                                type="password"
                                name="Password"
                                value={formData.Password}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Confirm Password
                            <input
                                type="password"
                                name="confirmPassword"
                                value={confirmPassword}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                </div>

                {/* Widoczność */}
                <div className={styles.block} onClick={() => handleToggle('widoczność')}>
                    <FaEye className={styles.icon}/>
                    Widoczność
                    {expandedBlocks.widoczność ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div className={`${styles.expandedContent} ${expandedBlocks.widoczność ? styles.expanded : ''}`}>
                    <div className={styles.subBlock}>
                        <label>
                            {/* Content for visibility section */}
                        </label>
                    </div>
                </div>

                {/* Dane płatnicze */}
                <div className={styles.block} onClick={() => handleToggle('danePłatnicze')}>
                    <FaCreditCard className={styles.icon}/>
                    Dane płatnicze
                    {expandedBlocks.danePłatnicze ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div className={`${styles.expandedContent} ${expandedBlocks.danePłatnicze ? styles.expanded : ''}`}>
                    <div className={styles.subBlock}>
                        <label>
                            Credit Card Number
                            <input
                                type="text"
                                name="creditCardNumber"
                                value={formData.creditCardNumber}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Expiry Date
                            <input
                                type="text"
                                name="expiryDate"
                                value={formData.expiryDate}
                                onChange={handleChange}
                                className={styles.input}
                                placeholder="MM/YY"
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            CVV
                            <input
                                type="text"
                                name="cvv"
                                value={formData.cvv}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                </div>
                <button className={styles.saveButton} onClick={handleSaveChanges}>Zapisz zmiany</button>
            </div>
        </div>
    );
}
