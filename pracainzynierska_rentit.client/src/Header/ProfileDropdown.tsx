import React, { forwardRef } from "react";
import styles from './ProfileDropdown.module.css';

interface ProfileDropdownProps {
    position: {
        top: number;
        right: number;
    };
}

export const ProfileDropdown = forwardRef<HTMLDivElement, ProfileDropdownProps>(
    ({ position }, ref) => {
        return (
            <div className={styles.dropdown} style={{ top: position.top, right: position.right }} ref={ref}>
                <ul className={styles.menu}>
                    <li className={styles.menuItem}>Zaloguj się</li>
                    <li className={styles.menuItem}>Zarejestruj się</li>
                    <li className={styles.menuItem}>Centrum pomocy</li>
                </ul>
            </div>
        );
    }
);
