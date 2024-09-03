import React, { forwardRef } from "react";
import styles from './ProfileDropdown.module.css';

interface ProfileDropdownProps {
    position: {
        top: number;
        right: number;
    };
    onOpenModal: () => void; 
    isLoggedIn: boolean; 
}

export const ProfileDropdown = forwardRef<HTMLDivElement, ProfileDropdownProps>(
    ({ position, onOpenModal, isLoggedIn }, ref) => {
        return (
            <div className={styles.dropdown} style={{ top: position.top, right: position.right }} ref={ref}>
                <ul className={styles.menu}>
                    {isLoggedIn ? (
                        <>
                            <li className={styles.menuItem} onClick={() => window.location.href = "/Settings"}>Ustawienia</li>
                            <li className={styles.menuItem}>Dodaj swoje mieszkanie</li>
                            <li className={styles.menuItem}>Dodaj swoje ogłoszenie</li>
                            <li className={styles.menuItem}>Twój test osobowości</li>
                            <li className={styles.menuItem}>Centrum pomocy</li>
                            <li className={styles.menuItem}>Wyloguj się</li>
                        </>
                    ) : (
                        <>
                            <li className={styles.menuItem} onClick={onOpenModal}>Zaloguj się</li>
                            <li className={styles.menuItem} onClick={onOpenModal}>Zarejestruj się</li>
                            <li className={styles.menuItem}>Centrum pomocy</li>
                        </>
                    )}
                </ul>
            </div>
        );
    }
);
