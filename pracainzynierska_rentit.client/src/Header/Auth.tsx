import React, { useEffect, useRef, useState } from "react";
import styles from './Auth.module.css';
import { Button, TextInput } from "@mantine/core";
import { LoginSocialFacebook, LoginSocialGoogle } from 'reactjs-social-login';
import { FacebookLoginButton, GoogleLoginButton } from "react-social-login-buttons";

interface AuthModalProps {
    onClose: () => void;
}

export function AuthModal({ onClose }: AuthModalProps) {
    const modalRef = useRef<HTMLDivElement>(null);
    const [email, setEmail] = useState<string>("");

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

    const handleButtonClick = () => {
        console.log(email);
    };

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent} ref={modalRef}>
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
            </div>
        </div>
    );
}
