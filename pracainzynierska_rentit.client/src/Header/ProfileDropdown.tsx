import React, { forwardRef } from "react";
import styles from './ProfileDropdown.module.css';

interface ProfileDropdownProps {
    position: {
        top: number;
        right: number;
    };
    onOpenModal: () => void; // New prop to open the modal
}

export const ProfileDropdown = forwardRef<HTMLDivElement, ProfileDropdownProps>(
    ({ position, onOpenModal }, ref) => {
        return (
            <div className={styles.dropdown} style={{ top: position.top, right: position.right }} ref={ref}>
                <ul className={styles.menu}>
                    <li className={styles.menuItem} onClick={onOpenModal}>Zaloguj się</li>
                    <li className={styles.menuItem} onClick={onOpenModal}>Zarejestruj się</li>
                    <li className={styles.menuItem}>Centrum pomocy</li>
                </ul>
            </div>
        );
    }
);
