// @ts-ignore
import React, { useEffect, useRef } from "react";
import styles from './BigModal.module.css';

interface BigModalProps {
    onClose: () => void;
}

export function BigModal({ onClose }: BigModalProps) {
    const modalRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (modalRef.current && !modalRef.current.contains(event.target as Node)) {
                onClose();
            }
        };

        document.addEventListener('mousedown', handleClickOutside);

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [onClose]);

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent} ref={modalRef}>
                <button onClick={onClose} className={styles.closeButton}>X</button>
                
            </div>
        </div>
    );
}
