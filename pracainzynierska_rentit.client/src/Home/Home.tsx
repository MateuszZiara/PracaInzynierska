import {Header} from "../Header/Header.tsx";
import styles from "./Home.module.css"
import React, {useEffect, useRef, useState} from "react";
import {FaFacebookF, FaInstagram, FaTwitter} from "react-icons/fa";
import {Footer} from "./Footer.tsx";
export function Home()
{
    const sectionsRef = useRef([]);
    const [currentSection, setCurrentSection] = useState("section1");
    const images = {
        section1: './src/assets/Section1-Photo.jpg',
        section2: './src/Home/Section2-Photo.jpg',
        section3: './src/Home/Section3-Photo.jpg',
    };
    const [backgroundImage, setBackgroundImage] = useState('./Section1-Photo.jpg');
    useEffect(() => {
        const observerOptions = {
            threshold: 0.5,
        };
        const observer = new IntersectionObserver((entries) => {
            entries.forEach((entry) => {
                const target = entry.target;
                if (entry.isIntersecting) {
                    target.classList.add(styles.visible);
                    setCurrentSection(target.dataset.section);
                } else {
                    target.classList.remove(styles.visible);
                }
            });
        }, observerOptions);

        sectionsRef.current.forEach((section) => {
            observer.observe(section);
        });

        return () => {
            sectionsRef.current.forEach((section) => {
                observer.unobserve(section);
            });
        };
    }, []);
    useEffect(() => {
        const image = images[currentSection] || './Section1-Photo.jpg';
        console.log('Current Section:', currentSection);
        console.log('Background Image:', image);
        setBackgroundImage(image);
    }, [currentSection]);
    const scrollToSection1 = () => {
        if (sectionsRef.current[1]) {
            sectionsRef.current[1].scrollIntoView({ behavior: 'smooth' });
        }
    };
    const sections = [
        { id: "section1", label: "Ogłoszenia" },
        { id: "section2", label: "Test osobowości" },
        { id: "section3", label: "Mieszkania" },
    ];
    return(
        <div className={styles.home}>
            <div className={styles.container} style={{backgroundImage: `url(${backgroundImage})`}}>
                {Header()}
                <div className={styles.main}>
                    <section
                        ref={(el) => (sectionsRef.current[1] = el)}
                        data-section="section1"
                        className={`${styles.section} ${styles.section1}`}
                    >
                        <div className={`${styles.animatedText} ${styles.animatedText1}`}>
                            Poznaj nowe osoby, załóż z nimi grupe i szukajcie mieszkania razem!
                        </div>
                        <div className={styles.buttoncustom}>
                            Przejdź do ogłoszeń
                        </div>
                    </section>
                    <section
                        ref={(el) => (sectionsRef.current[2] = el)}
                        data-section="section2"
                        className={`${styles.section} ${styles.section2}`}
                    >
                        <div className={`${styles.animatedText} ${styles.animatedText2}`}>
                            <div>Weź udział w teście osobowości i znajdź ludzi podobnych do siebie</div>
                        </div>
                        <div className={styles.buttoncustom}>
                            Przejdź do testu
                        </div>
                    </section>
                    <section
                        ref={(el) => (sectionsRef.current[3] = el)}
                        data-section="section3"
                        className={`${styles.section} ${styles.section3}`}
                    >
                        <div className={`${styles.animatedText} ${styles.animatedText3}`}>
                            <div>Masz już grupę przyjaciół? Znajdźcie razem mieszkanie przeglądając dostępne oferty
                            </div>
                        </div>
                        <div className={styles.buttoncustom}>
                            Sprawdź mieszkania
                        </div>
                    </section>
                </div>
                <aside className={styles.tableOfContents}>
                    {sections.map((section) => (
                        <div
                            key={section.id}
                            className={`${styles.tocItem} ${
                                currentSection === section.id ? styles.activeSection : ""
                            }`}
                            onClick={() => {
                                const sectionElement = sectionsRef.current.find((el) => el?.dataset.section === section.id);
                                sectionElement?.scrollIntoView({behavior: "smooth"});
                            }}
                        >
                            <span>{section.label}</span>
                            <div className={styles.square}></div>
                        </div>
                    ))}
                </aside>
                <Footer />
            </div>
        </div>
    )
}