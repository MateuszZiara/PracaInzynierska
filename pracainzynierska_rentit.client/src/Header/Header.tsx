import React, { useState, useRef, useEffect } from "react";
import { FaBars } from "react-icons/fa";
import { Avatar } from "@mantine/core";
import { AuthModal } from "./Auth";
import styles from './Header.module.css';
import { ProfileDropdown } from "./ProfileDropdown";
import { BigModal } from "./BigModal";
import { useLocation } from "react-router-dom";

export function Header() {
    const [isModalOpen, setModalOpen] = useState(false);
    const [isModalOpenLeft, setModalOpenLeft] = useState(false);
    const [isDropdownOpen, setDropdownOpen] = useState(false);
    const [dropdownPosition, setDropdownPosition] = useState({ top: 0, right: 0 });
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');

    const location = useLocation();

    const profileRef = useRef<HTMLDivElement>(null);
    const dropdownRef = useRef<HTMLDivElement>(null);

    const handleOpenModal = () => setModalOpen(true);
    const handleOpenModalLeft = () => setModalOpenLeft(true);
    const handleCloseModal = () => setModalOpen(false);
    const handleCloseModalLeft = () => setModalOpenLeft(false);

    const toggleDropdown = () => {
        if (profileRef.current) {
            const rect = profileRef.current.getBoundingClientRect();
            setDropdownPosition({
                top: rect.bottom + 15,
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
                handleCloseModalLeft();
                closeDropdown();
            }
        };

        document.addEventListener('keydown', handleKeyDown);

        return () => {
            document.removeEventListener('keydown', handleKeyDown);
        };
    }, []);

    useEffect(() => {
        const checkUserLoggedIn = async () => {
            try {
                const response = await fetch("api/AspNetUsers/info", {
                    credentials: 'include',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                });
                if (response.ok) {
                    const data = await response.json();
                    setFirstName(data.firstName);
                    setLastName(data.lastName);
                    setIsLoggedIn(true);
                } else {
                    console.log("Niezalogowany");
                    setIsLoggedIn(false);
                }
            } catch (error) {
                console.error("Error checking login status:", error);
                setIsLoggedIn(false);
            }
        };
        checkUserLoggedIn();
    }, []);

    const initials = `${firstName.charAt(0)}${lastName.charAt(0)}`; // Create initials from firstName and lastName

    return (
        <div className={styles.header}>
            <FaBars className={styles.hamburger} onClick={handleOpenModalLeft} />
            <div className={styles.logo} onClick={() => window.location.href ="/"}>
                <span className={styles.rent}>Rent</span>
                <span className={styles.it}>It</span>
            </div>
            <div className={styles.navigation}>
                <span className={location.pathname === "/"  ? styles.active : styles.unactive} onClick={() => window.location.href = "/"}>Główna</span>
                <span className={location.pathname === "/mieszkania" ? styles.active : styles.unactive} onClick={() => window.location.href = "/mieszkania"}>Mieszkania</span>
                <span className={location.pathname === "/osoby" ? styles.active : styles.unactive} onClick={() => window.location.href = "/osoby"}>Osoby</span>
                <span className={location.pathname === "/swipeit" ? styles.active : styles.unactive} onClick={() => window.location.href = "/swipeit"}>SwipeIt!</span>
                
            </div>
            <div className={styles.profile} onClick={toggleDropdown} ref={profileRef}>
                <FaBars className={styles.hamburgerProfile} />
                <div className={styles.avatar}>
                    {isLoggedIn ? (
                        <Avatar
                            color="initials"
                            radius="xl"
                            size="md"
                            name={`${firstName} ${lastName}`} // Pass full name to the Avatar
                            className={styles.avatarInitials}
                        />
                    ) : (
                        <Avatar variant="transparent" radius="xl" size="lg" />
                    )}
                </div>
                {isDropdownOpen && (
                    <ProfileDropdown
                        ref={dropdownRef}
                        position={dropdownPosition}
                        onOpenModal={handleOpenModal}
                        isLoggedIn={isLoggedIn} // Pass the login state
                    />
                )}
            </div>
            {isModalOpenLeft && <BigModal onClose={handleCloseModalLeft} />}
            {isModalOpen && <AuthModal onClose={handleCloseModal} />}
        </div>
    );
}
