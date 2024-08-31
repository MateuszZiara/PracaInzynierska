import React, { useState, useRef, useEffect } from "react";
import { FaBars } from "react-icons/fa";
import { Avatar } from "@mantine/core";
import { BigModal } from "./BigModal";
import { ProfileDropdown } from "./ProfileDropdown";
import styles from './Header.module.css';

export function Header() {
    const [isModalOpen, setModalOpen] = useState(false);
    const [isDropdownOpen, setDropdownOpen] = useState(false);
    const [dropdownPosition, setDropdownPosition] = useState({ top: 0, right: 0 });

    const profileRef = useRef<HTMLDivElement>(null);
    const dropdownRef = useRef<HTMLDivElement>(null);

    const handleOpenModal = () => setModalOpen(true);
    const handleCloseModal = () => setModalOpen(false);

    const toggleDropdown = () => {
        if (profileRef.current) {
            const rect = profileRef.current.getBoundingClientRect();
            setDropdownPosition({
                top: rect.bottom + 15, // 15px below the profile div
                right: window.innerWidth - rect.right,
            });
        }
        setDropdownOpen(prevState => !prevState);
    };

    const closeDropdown = () => setDropdownOpen(false);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target as Node) &&
                profileRef.current &&
                !profileRef.current.contains(event.target as Node)
            ) {
                closeDropdown();
            }
        };

        document.addEventListener('mousedown', handleClickOutside);

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, []);

    useEffect(() => {
        const handleKeyDown = (event: KeyboardEvent) => {
            if (event.key === 'Escape') {
                handleCloseModal();
                closeDropdown();
            }
        };

        document.addEventListener('keydown', handleKeyDown);

        return () => {
            document.removeEventListener('keydown', handleKeyDown);
        };
    }, []);

    return (
        <div className={styles.header}>
            <FaBars className={styles.hamburger} onClick={handleOpenModal} />
            <div className={styles.logo}>
                <span className={styles.rent}>Rent</span>
                <span className={styles.it}>It</span>
            </div>
            <div className={styles.navigation}>
                <span className={styles.active}>Główna</span>
                <span className={styles.unactive}>Mieszkania</span>
                <span className={styles.unactive}>Osoby</span>
                <span className={styles.unactive}>SwipeIt!</span>
            </div>
            <div className={styles.profile} onClick={toggleDropdown} ref={profileRef}>
                <FaBars className={styles.hamburgerProfile} />
                <div className={styles.avatar}>
                    <Avatar variant="transparent" radius="xl" src="" size="lg" />
                </div>
                {isDropdownOpen && <ProfileDropdown ref={dropdownRef} position={dropdownPosition} />}
            </div>
            {isModalOpen && <BigModal onClose={handleCloseModal} />}
        </div>
    );
}
