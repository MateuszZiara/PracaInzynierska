import React from "react";
import styles from "./Footer.module.css";
import {FaFacebookF, FaInstagram, FaTwitter} from "react-icons/fa";

export function Footer() {
    return (
        <footer className={styles.footer}>
            <div className={styles.footerContent}>
                <div className={styles.left}>
                    <p  className={styles.text}>&copy; 2024 RentIt. inc.</p>
                    <p className={styles.text}>Kontakt</p>
                    <p  className={styles.text}>Centrum pomocy</p>
                </div>
                <div className={styles.socialIcons}>
                    <FaFacebookF />
                    <FaInstagram />
                    <FaTwitter />
                </div>
            </div>
        </footer>
    );
}
