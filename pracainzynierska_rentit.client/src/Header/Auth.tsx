import React, { useEffect, useRef, useState } from "react";
import styles from './Auth.module.css';
import { Button, TextInput, Loader, PasswordInput } from "@mantine/core";
import { LoginSocialFacebook, LoginSocialGoogle } from 'reactjs-social-login';
import { FacebookLoginButton, GoogleLoginButton } from "react-social-login-buttons";
import { DateInput } from 'rsuite';
import 'rsuite/dist/rsuite.min.css'; // Import RSuite CSS for DateInput styling

interface AuthModalProps {
    onClose: () => void;
}

export function AuthModal({ onClose }: AuthModalProps) {
    const modalRef = useRef<HTMLDivElement>(null);
    const [email, setEmail] = useState<string>("");
    const [firstname, setfirstname] = useState<string>("");
    const [lastname, setlastname] = useState<string>("");
    const [password, setpassword] = useState<string>("");
    const [isExistingEmail, setIsExistingEmail] = useState<boolean | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [birthValue, setBirthValue] = useState<Date | null>(null); // Updated state name to camelCase

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (modalRef.current && !modalRef.current.contains(event.target as Node)) {
                onClose();
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

    const handleButtonClick = async () => {
        setLoading(true); // Start loading
        setIsExistingEmail(null); // Clear previous state

        try {
            const response = await fetch(`https://localhost:7214/checkEmail?email=${email}`);
            const result = await response.json();

            if (result.exists) {
                setIsExistingEmail(true); // Email exists, show "Log In"
            } else {
                setIsExistingEmail(false); // Email does not exist, show "Register"
            }
        } catch (error) {
            console.error("Error checking email:", error);
            setIsExistingEmail(null); // Clear state on error
        } finally {
            setLoading(false); // Stop loading
        }
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
                        {isExistingEmail ?
                            (
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
                                            onChange={(event) => setpassword(event.currentTarget.value)}
                                        />
                                    </div>
                                </>
                            )
                            :
                            (
                                <>
                                    <div className={styles.header}>
                                        <p className={styles.headerText}>Zarejestruj się</p>
                                    </div>
                                    <div className={styles.email}>
                                        <TextInput
                                            label="Imię"
                                            size="lg"
                                            radius="md"
                                            placeholder="Imię"
                                            value={firstname}
                                            onChange={(event) => setfirstname(event.currentTarget.value)}
                                        />
                                    </div>
                                    <div className={styles.email}>
                                        <TextInput
                                            label="Nazwisko"
                                            size="lg"
                                            radius="md"
                                            placeholder="Nazwisko"
                                            value={lastname}
                                            onChange={(event) => setlastname(event.currentTarget.value)}
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
                                            Aby się zarejestrować musisz mieć ukończone 18 lat. Zgadzasz się na udostępnianie twojego wieku innym.
                                        </span>
                                    </div>
                                    <div className={styles.email}>
                                        <PasswordInput
                                            size="lg"
                                            radius="md"
                                            placeholder="Hasło"
                                            label="Hasło"
                                            value={password}
                                            onChange={(event) => setpassword(event.currentTarget.value)}
                                        />
                                    </div>
                                    <div className={styles.email}>
                                        <span className={styles.info}>
                                                Klikając zarejestruj wyrażasz zgodę na przetwarzanie twoich danych osobowych.
                                        </span>
                                    </div>
                                    <div className={styles.next}>
                                        <Button
                                            fullWidth
                                            variant="filled"
                                            color="rgba(127, 56, 181, 1)"
                                            size="lg"
                                            onClick={handleButtonClick}
                                        >
                                            Zarejestruj się
                                        </Button>
                                    </div>
                                </>
                            )
                        }
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
