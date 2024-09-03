import React, { useState } from 'react';
import styles from './settings.module.css';
import {FaUser, FaLock, FaEye, FaCreditCard, FaChevronDown, FaChevronUp, FaCheckCircle} from 'react-icons/fa';
import {Header} from "../Header/Header.tsx";

type BlockState = {
    daneOsobowe: boolean;
    logowanie: boolean;
    widoczność: boolean;
    danePłatnicze: boolean;
};

export default function Settings() {
    const [expandedBlocks, setExpandedBlocks] = useState<BlockState>({
        daneOsobowe: false,
        logowanie: false,
        widoczność: false,
        danePłatnicze: false
    });

    // State for the user inputs
    const [formData, setFormData] = useState({
        imie: '',
        nazwisko: '',
        email: '',
        dataUrodzenia: '',
        telefonKomorkowy: '',
        newPassword: '',
        confirmPassword: '',
        profileVisibility: 'Public',
        creditCardNumber: '',
        expiryDate: '',
        cvv: ''
    });

    // Handle input changes
    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    // Toggle function for expanding and collapsing blocks
    const handleToggle = (block: keyof BlockState) => {
        setExpandedBlocks(prevState => ({
            ...prevState,
            [block]: !prevState[block]
        }));
    };

    // Save changes handler
    const handleSaveChanges = () => {
        // Handle save logic here
        alert('Changes saved!');
    };

    return (
        <div className={styles.site}>
            {Header()}
            <div className={styles.container}>
                <div className={styles.verifyBlock}>
                    <FaCheckCircle className={styles.verification}/>
                    <div className={styles.textBlock}>
                        <div className={styles.mainText}>Zweryfikuj swój email</div>
                        <div className={styles.subText}>Zweryfikuj swój email, aby móc korzystać w pełni z naszej
                            strony
                        </div>
                    </div>
                    <button className={styles.verifyButton}>Zweryfikuj konto</button>
                </div>


                <div className={styles.block} onClick={() => handleToggle('daneOsobowe')}>
                    <FaUser className={styles.icon}/>
                    Dane osobowe
                    {expandedBlocks.daneOsobowe ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div
                    className={`${styles.expandedContent} ${expandedBlocks.daneOsobowe ? styles.expanded : ''}`}
                >
                    <div className={styles.subBlock}>
                        <label>
                            Imie
                            <input
                                type="text"
                                name="imie"
                                value={formData.imie}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <div className={styles.subBlock}>
                        <label>
                            Nazwisko
                            <input
                                type="text"
                                name="nazwisko"
                                value={formData.nazwisko}
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
                                name="email"
                                value={formData.email}
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
                                name="dataUrodzenia"
                                value={formData.dataUrodzenia}
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
                                name="telefonKomorkowy"
                                value={formData.telefonKomorkowy}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <button className={styles.saveButton} onClick={handleSaveChanges}>Zapisz zmiany</button>
                </div>

                <div className={styles.block} onClick={() => handleToggle('logowanie')}>
                    <FaLock className={styles.icon}/>
                    Logowanie
                    {expandedBlocks.logowanie ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div
                    className={`${styles.expandedContent} ${expandedBlocks.logowanie ? styles.expanded : ''}`}
                >
                    <div className={styles.subBlock}>
                        <label>
                            New Password
                            <input
                                type="password"
                                name="newPassword"
                                value={formData.newPassword}
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
                                value={formData.confirmPassword}
                                onChange={handleChange}
                                className={styles.input}
                            />
                        </label>
                    </div>
                    <button className={styles.saveButton} onClick={handleSaveChanges}>Zapisz zmiany</button>
                </div>

                <div className={styles.block} onClick={() => handleToggle('widoczność')}>
                    <FaEye className={styles.icon}/>
                    Widoczność
                    {expandedBlocks.widoczność ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div
                    className={`${styles.expandedContent} ${expandedBlocks.widoczność ? styles.expanded : ''}`}
                >
                    <div className={styles.subBlock}>
                        <label>
                            Profile Visibility
                            <select
                                name="profileVisibility"
                                value={formData.profileVisibility}
                                onChange={handleChange}
                                className={styles.input}
                            >
                                <option value="Public">Public</option>
                                <option value="Friends">Friends</option>
                                <option value="Private">Private</option>
                            </select>
                        </label>
                    </div>
                    <button className={styles.saveButton} onClick={handleSaveChanges}>Zapisz zmiany</button>
                </div>

                <div className={styles.block} onClick={() => handleToggle('danePłatnicze')}>
                    <FaCreditCard className={styles.icon}/>
                    Dane płatnicze
                    {expandedBlocks.danePłatnicze ? <FaChevronUp className={styles.toggleIcon}/> :
                        <FaChevronDown className={styles.toggleIcon}/>}
                </div>

                <div
                    className={`${styles.expandedContent} ${expandedBlocks.danePłatnicze ? styles.expanded : ''}`}
                >
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
                    <button className={styles.saveButton} onClick={handleSaveChanges}>Zapisz zmiany</button>
                </div>
            </div>
        </div>
    );
}
