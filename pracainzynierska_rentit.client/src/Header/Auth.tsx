﻿import React, { useEffect, useRef, useState } from "react";
import styles from './Auth.module.css';
import {Button, TextInput, Loader, PasswordInput, Autocomplete, MultiSelect} from "@mantine/core";
import axios from 'axios';
import { LoginSocialFacebook, LoginSocialGoogle } from 'reactjs-social-login';
import { FacebookLoginButton, GoogleLoginButton } from "react-social-login-buttons";
import { DateInput } from 'rsuite';
import 'rsuite/dist/rsuite.min.css';

interface AuthModalProps {
    onClose: () => void;
}

export function AuthModal({ onClose }: AuthModalProps) {
    const modalRef = useRef<HTMLDivElement>(null);
    const [email, setEmail] = useState<string>("");
    const [Id, setId] = useState();
    const [firstname, setFirstname] = useState<string>("");
    const [lastname, setLastname] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [isExistingEmail, setIsExistingEmail] = useState<boolean | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [birthValue, setBirthValue] = useState<Date | null>(null);
    const [emailError, setEmailError] = useState<string | null>(null);
    const [passwordError, setPasswordError] = useState<string | null>(null);
    const [firstnameError, setFirstnameError] = useState<string | null>(null);
    const [lastnameError, setLastnameError] = useState<string | null>(null);
    const [birthDateError, setBirthDateError] = useState<string | null>(null);
    const [localizations, setLocalizations] = useState<string[]>([]);
    const [localizationsSelected, setLocalizationsSelected] = useState<string[]>([]);
    const [localizationError, setLocalizationError] = useState<string | null>(null);
    const dropdownOpenRef = useRef<boolean>(false);
    const [localizationsData, setLocalizationsData] = useState<string[]>([]);
    useEffect(() => {
        const fetchLocalizations = async () => {
            try {
                const url = `api/Localization/GetAll`;
                const response = await axios.get(url);
                const localizationData = response.data;

                const cityNames = Array.from(new Set(
                    localizationData.map((item) => `${item.name}, ${item.province}`)
                ));

                setLocalizationsData(localizationData); // Store complete data
                setLocalizations(cityNames);
            } catch (error) {
                console.error('Error fetching localizations:', error);
                setLocalizationError('An error occurred while fetching localizations.');
            }
        };

        fetchLocalizations();
    }, []);



    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (modalRef.current && !modalRef.current.contains(event.target as Node)) {
                if (!dropdownOpenRef.current) {
                    onClose();
                }
            }
        };

        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === 'Escape') {
                onClose();
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        document.addEventListener('keydown', handleKeyDown);

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
            document.removeEventListener('keydown', handleKeyDown);
        };
    }, [onClose]);

    const validateEmail = (email: string): boolean => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    };
    
    const validatePassword = (password: string): boolean => {
        const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
        return passwordRegex.test(password);
    };

    const handleButtonClick = async () => {
        setLoading(true);
        setIsExistingEmail(null);
        setEmailError(null);

        if (!validateEmail(email)) {
            setEmailError("Nieprawidłowy adres email");
            setLoading(false);
            return;
        }

        try {
            const response = await fetch(`api/AspNetUsers/checkEmail?email=${email}`);
            const result = await response.json();
            setIsExistingEmail(result);
        } catch (error) {
            console.error("Error checking email:", error);
            setEmailError("An error occurred while checking the email.");
            setIsExistingEmail(null);
        } finally {
            setLoading(false);
        }
    };

    const handleRegisterClick = async () => {
        setLoading(true);
        setPasswordError(null);
        setFirstnameError(null);
        setLastnameError(null);
        setBirthDateError(null);
        setLocalizationError(null);
        let hasError = false;

        if (birthValue) {
            const today = new Date();
            const birthDate = new Date(birthValue);
            let age = today.getFullYear() - birthDate.getFullYear();
            const monthDifference = today.getMonth() - birthDate.getMonth();

            if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }

            if (age < 18) {
                setBirthDateError("Musisz mieć co najmniej 18 lat, aby się zarejestrować.");
                hasError = true;
            }
        } else {
            setBirthDateError("Data urodzenia nie może być pusta.");
            hasError = true;
        }

        if (!validatePassword(password)) {
            setPasswordError("Hasło musi zawierać przynajmniej jedną dużą literę, jedną cyfrę oraz jeden znak specjalny.");
            hasError = true;
        }

        if (!firstname) {
            setFirstnameError("Imię nie może być puste.");
            hasError = true;
        }

        if (!lastname) {
            setLastnameError("Nazwisko nie może być puste.");
            hasError = true;
        }

        if (localizationsSelected.length === 0) {
            setLocalizationError("Musisz wybrać przynajmniej jedną lokalizację.");
            hasError = true;
        }

        if (hasError) {
            setLoading(false);
            return;
        }

        const selectedIds = localizationsSelected.map((city) => {
            const localization = localizationsData.find((item) => `${item.name}, ${item.province}` === city);
            return localization ? localization.id : null;
        }).filter(id => id !== null);

        try {
            // Set the time to midnight UTC to avoid time zone issues
            const birthDateUTC = birthValue ? new Date(Date.UTC(birthValue.getFullYear(), birthValue.getMonth(), birthValue.getDate())) : null;

            // Format the date to 'YYYY-MM-DD'
            const birthDateFormatted = birthDateUTC ? birthDateUTC.toISOString().split('T')[0] : null;

            const registerData = {
                Email: email,
                Password: password,
                FirstName: firstname,
                LastName: lastname,
                BirthDay: birthDateFormatted, // Use the formatted date
                Provider: "Website"
            };

            console.log(registerData);

            const response = await fetch("api/AspNetUsers/Register", {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(registerData),
            });

            if (response.ok) {
                const loginData = {
                    Email: email,
                    Password: password,
                };
                const responseLogin = await fetch("loginCustomWebsite?useCookies=true&useSessionCookies=false", {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(loginData),
                    credentials: 'include'
                });
                if (!responseLogin.ok) {
                    const errorMessage = await responseLogin.text();
                    setPasswordError("Złe hasło");
                    throw new Error(`HTTP error! Status: ${responseLogin.status}, Message: ${errorMessage}`);
                } else {
                    const registerLocations = {
                        localizationId: selectedIds
                    }
                    const response = await fetch('api/LocalizationUser/CreateMany', {
                        method: 'POST',
                        headers: {'Content-Type': 'application/json'},
                        body: JSON.stringify(registerLocations),
                    });
                    if (response.ok) {
                        window.location.reload();
                    } else {
                        console.error('Error during registration:', await response.text());
                    }
                }
            } else {
                console.error('Error during registration:', await response.text());
            }
        } catch (error) {
            console.error('Error during registration:', error);
        } finally {
            setLoading(false);
        }
    };



    const handleLoginClick = async () => {
        const loginData = {
            Email: email,
            Password: password,
        };
        const response = await fetch("loginCustomWebsite?useCookies=true&useSessionCookies=false", {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(loginData),
            credentials: 'include'
        });
        if (!response.ok) {
            const errorMessage = await response.text();
            setPasswordError("Złe hasło");
            throw new Error(`HTTP error! Status: ${response.status}, Message: ${errorMessage}`);
        } else {
            window.location.reload();
        }
    }
    const handleLocalizationChange = (selectedCities) => {
        setLocalizationsSelected(selectedCities);
    };
    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent} ref={modalRef}>
                {loading ? (
                    <div className={styles.loaderContainer}>
                        <Loader />
                        <p>Loading...</p>
                    </div>
                ) : isExistingEmail !== null ? (
                    <div className={styles.secondContent}>
                        {isExistingEmail ? (
                            <>
                                <div className={styles.header}>
                                    <p className={styles.headerText}>Zaloguj się</p>
                                </div>
                                <div className={styles.email}>
                                    <PasswordInput
                                        size="lg"
                                        radius="md"
                                        placeholder="Hasło"
                                        value={password}
                                        onChange={(event) => setPassword(event.currentTarget.value)}
                                        error={passwordError}
                                    />
                                </div>
                                <div className={styles.next}>
                                    <Button
                                        fullWidth
                                        variant="filled"
                                        color="rgba(127, 56, 181, 1)"
                                        size="lg"
                                        onClick={handleLoginClick}
                                    >
                                        Zaloguj się
                                    </Button>
                                </div>
                                <div className={styles.divider}>
                                    <span>lub</span>
                                </div>
                                <div className={styles['login-social']}>
                                    <LoginSocialFacebook
                                        appId="310778082068786"
                                        onReject={(error) => {
                                            console.log(error);
                                        }}
                                        onResolve={async (response) => {
                                        }}
                                    >
                                        <FacebookLoginButton />
                                    </LoginSocialFacebook>
                                    <div className={styles.google}>
                                        <LoginSocialGoogle
                                            appId="310778082068786"
                                            onReject={(error) => {
                                                console.log(error);
                                            }}
                                            onResolve={async (response) => {
                                            }}
                                        >
                                            <GoogleLoginButton />
                                        </LoginSocialGoogle>
                                    </div>
                                </div>
                            </>
                        ) : (
                            <>
                                <div className={styles.header}>
                                    <p className={styles.headerText}>Rejestracja</p>
                                </div>
                                <div className={styles.email}>
                                    <TextInput
                                        size="lg"
                                        radius="md"
                                        placeholder="Imię"
                                        label="Imię"
                                        value={firstname}
                                        onChange={(event) => setFirstname(event.currentTarget.value)}
                                        error={firstnameError}
                                    />
                                </div>
                                <div className={styles.email}>
                                    <TextInput
                                        size="lg"
                                        radius="md"
                                        placeholder="Nazwisko"
                                        label="Nazwisko"
                                        value={lastname}
                                        onChange={(event) => setLastname(event.currentTarget.value)}
                                        error={lastnameError}
                                    />
                                </div>
                                <div className={styles.email}>
                                    <label>Data urodzenia</label>
                                    <DateInput
                                        value={birthValue}
                                        onChange={setBirthValue}
                                        placeholder="Data urodzenia"
                                        size="lg"
                                    />
                                    <span className={styles.info}>
                                        {birthDateError && <p className={styles.error}>Aby się zarejestrować musisz mieć ukończone 18 lat. Zgadzasz się na udostępnianie twojego wieku innym.</p>}
                                    </span>
                                </div>
                                <div className={styles.email}>
                                    <MultiSelect
                                        label="Wybierz lokalizacje"
                                        data={localizations} // Update with your actual data
                                        styles={(theme) => ({
                                            dropdown: {
                                                zIndex: 2000,
                                            },
                                        })}
                                        searchable
                                        onChange={handleLocalizationChange} // Use the new handler
                                        onDropdownOpen={() => { dropdownOpenRef.current = true; }}
                                        onDropdownClose={() => { dropdownOpenRef.current = false; }}
                                        size="lg"
                                        maxValues={5}
                                        comboboxProps={{ transitionProps: { transition: 'pop', duration: 200 } }}
                                    />
                                    <span className={styles.info}>
                                        Wybierz lokalizacje, do których chcesz się przeprowadzić. Spokojnie później możesz to zmienić w ustawieniach!
                                    </span>
                                </div>
                                <div className={styles.email}>
                                    <PasswordInput
                                        size="lg"
                                        radius="md"
                                        placeholder="Hasło"
                                        label="Hasło"
                                        value={password}
                                        onChange={(event) => setPassword(event.currentTarget.value)}
                                        error={passwordError}
                                    />
                                    <span className={styles.info}>
                                        Hasło musi zawierać przynajmniej jedną dużą literę, jedną cyfrę oraz jeden znak specjalny.
                                    </span>
                                </div>
                                <div className={styles.email}>
                                </div>
                                <div className={styles.next}>
                                    <Button
                                        fullWidth
                                        variant="filled"
                                        color="rgba(127, 56, 181, 1)"
                                        size="lg"
                                        onClick={handleRegisterClick}
                                    >
                                        Zarejestruj się
                                    </Button>
                                    <span className={styles.info}>
                                        Klikając zarejestruj wyrażasz zgodę na przetwarzanie twoich danych osobowych.
                                    </span>
                                </div>
                            </>
                        )}
                    </div>
                ) : (
                    <>
                        <div className={styles.header}>
                            <p className={styles.headerText}>Zaloguj się lub zarejestruj</p>
                        </div>
                        <div className={styles.email}>
                            <TextInput
                                size="lg"
                                radius="md"
                                placeholder="Adres Email"
                                value={email}
                                onChange={(event) => setEmail(event.currentTarget.value)}
                                error={emailError}
                            />
                        </div>
                        <div className={styles.next}>
                            <Button
                                fullWidth
                                variant="filled"
                                color="rgba(127, 56, 181, 1)"
                                size="lg"
                                onClick={handleButtonClick}
                            >
                                Dalej
                            </Button>
                        </div>
                        <div className={styles.divider}>
                            <span>lub</span>
                        </div>
                        <div className={styles['login-social']}>
                            <LoginSocialFacebook
                                appId="310778082068786"
                                onReject={(error) => {
                                    console.log(error);
                                }}
                                onResolve={async (response) => {
                                }}
                            >
                                <FacebookLoginButton />
                            </LoginSocialFacebook>
                            <div className={styles.google}>
                                <LoginSocialGoogle
                                    appId="310778082068786"
                                    onReject={(error) => {
                                        console.log(error);
                                    }}
                                    onResolve={async (response) => {
                                    }}
                                >
                                    <GoogleLoginButton />
                                </LoginSocialGoogle>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
