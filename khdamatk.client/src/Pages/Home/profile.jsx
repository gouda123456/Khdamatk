import React from 'react';

const ProfilePage = () => {
  return (
    <>
      <style>{`
        :root {
          --primary-color: #7B1FA2;
          --dark-bg: #000000;
          --text-main: #111827;
          --text-muted: #6B7280;
          --bg-light: #F3F4F6;
          --border-color: #E5E7EB;
          --white: #FFFFFF;
          --danger: #EF4444;
          --font-family: 'Inter', sans-serif;
        }

        .profile-wrapper {
          background-color: #FAFAFA;
          color: var(--text-main);
          line-height: 1.6;
          font-family: var(--font-family);
          margin: 0;
          padding: 0;
          box-sizing: border-box;
        }

        .navbar {
          display: flex;
          justify-content: space-between;
          align-items: center;
          padding: 1rem 5%;
          background-color: var(--white);
          box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
          position: sticky;
          top: 0;
          z-index: 100;
        }

        .logo {
          font-size: 1.5rem;
          font-weight: 700;
          color: var(--primary-color);
        }

        .logo span {
          color: #F59E0B;
          font-weight: 400;
          border: 1px solid #F59E0B;
          padding: 0 4px;
          border-radius: 4px;
        }

        .nav-links {
          display: flex;
          gap: 2rem;
          font-weight: 600;
          list-style: none;
        }

        .nav-actions {
          display: flex;
          align-items: center;
          gap: 1.5rem;
          font-size: 1.2rem;
        }

        .user-avatar-small {
          width: 40px;
          height: 40px;
          border-radius: 50%;
          border: 2px solid var(--primary-color);
          display: flex;
          justify-content: center;
          align-items: center;
          color: var(--primary-color);
        }

        .cover-photo {
          height: 250px;
          background-color: var(--dark-bg);
          position: relative;
        }

        .cover-actions {
          position: absolute;
          top: 20px;
          right: 5%;
          display: flex;
          gap: 1rem;
          color: var(--white);
          font-size: 1.2rem;
          cursor: pointer;
        }

        .cover-actions .fa-trash { color: var(--danger); }

        .profile-container {
          max-width: 900px;
          margin: -100px auto 40px;
          background-color: var(--white);
          border-radius: 12px;
          box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
          padding: 0 2rem 2rem;
          position: relative;
        }

        .profile-header {
          display: flex;
          flex-direction: column;
          align-items: center;
          position: relative;
          padding-bottom: 2rem;
          border-bottom: 1px solid var(--border-color);
        }

        .avatar-container {
          position: relative;
          margin-top: -60px;
          margin-bottom: 1rem;
        }

        .profile-avatar {
          width: 150px;
          height: 150px;
          background-color: #D1D5DB;
          border-radius: 50%;
          border: 4px solid var(--white);
        }

        .avatar-actions {
          position: absolute;
          bottom: 10px;
          width: 100%;
          display: flex;
          justify-content: space-between;
          padding: 0 10px;
          font-size: 1.2rem;
        }

        .avatar-actions .fa-trash { color: var(--danger); cursor: pointer; }
        .avatar-actions .fa-pen { color: var(--text-main); cursor: pointer; }

        .profile-title-actions {
          position: absolute;
          top: 20px;
          right: 0;
          display: flex;
          gap: 1rem;
          font-size: 1.2rem;
          cursor: pointer;
        }

        .profile-name {
          font-size: 2rem;
          font-weight: 600;
          margin-bottom: 1.5rem;
        }

        .profile-stats {
          display: grid;
          grid-template-columns: repeat(3, 1fr);
          gap: 2rem;
          width: 100%;
          text-align: center;
          margin-bottom: 2rem;
        }

        .stat-item {
          display: flex;
          flex-direction: column;
          gap: 0.5rem;
        }

        .stars { color: var(--text-main); }

        .contact-btn {
          background-color: var(--primary-color);
          color: var(--white);
          border: none;
          padding: 0.8rem 2rem;
          border-radius: 8px;
          font-size: 1.1rem;
          font-weight: 500;
          cursor: pointer;
          transition: opacity 0.2s;
        }

        .pricing-row {
          display: flex;
          justify-content: space-between;
          padding: 1rem 0;
          border-bottom: 1px solid var(--border-color);
          font-weight: 500;
        }

        .bio-text {
          padding: 1.5rem 0;
          color: var(--text-main);
          font-size: 0.95rem;
        }

        .section-header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-top: 1.5rem;
          margin-bottom: 1rem;
          cursor: pointer;
        }

        .section-title {
          font-size: 1.2rem;
          font-weight: 600;
          display: flex;
          align-items: center;
          gap: 0.5rem;
        }

        .action-icon {
          color: var(--text-muted);
          cursor: pointer;
          font-size: 1.2rem;
        }

        .add-btn {
          background-color: var(--bg-light);
          width: 40px;
          height: 30px;
          display: flex;
          justify-content: center;
          align-items: center;
          border-radius: 6px;
          color: var(--primary-color);
          cursor: pointer;
        }

        .skills-list {
          display: flex;
          gap: 0.5rem;
          flex-wrap: wrap;
          margin-bottom: 1rem;
        }

        .skill-pill {
          background-color: #E5E7EB;
          padding: 0.4rem 1rem;
          border-radius: 20px;
          font-size: 0.85rem;
        }

        .portfolio-grid {
          display: grid;
          grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
          gap: 1.5rem;
          margin-bottom: 2rem;
        }

        .portfolio-card {
          border: 1px solid var(--border-color);
          border-radius: 8px;
          overflow: hidden;
          background: var(--white);
          box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
        }

        .portfolio-img {
          width: 100%;
          height: 140px;
          background-color: var(--dark-bg);
          object-fit: cover;
        }

        .portfolio-content { padding: 1rem; }

        .portfolio-title-row {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 0.5rem;
          font-weight: 600;
        }

        .timeline-item {
          display: flex;
          gap: 1.5rem;
          margin-bottom: 2rem;
          padding-left: 1rem;
        }

        .timeline-icon {
          font-size: 2rem;
          color: var(--text-main);
          margin-top: 0.5rem;
        }

        .timeline-content { flex: 1; }

        .timeline-title-row {
          display: flex;
          justify-content: space-between;
          align-items: flex-start;
          margin-bottom: 0.5rem;
        }

        .timeline-title { font-size: 1.2rem; font-weight: 600; }
        .timeline-subtitle { font-weight: 500; margin-bottom: 0.5rem; }
        .timeline-desc { color: var(--text-main); font-size: 0.95rem; margin-bottom: 1rem; }
        .timeline-date { font-weight: 500; color: var(--text-main); }

        .footer {
          background-color: #4A148C;
          color: var(--white);
          padding: 3rem 5%;
          margin-top: 4rem;
        }

        .footer-grid {
          display: grid;
          grid-template-columns: 1fr 1fr 1fr 1.5fr;
          gap: 2rem;
        }

        .footer-logo { font-size: 1.5rem; font-weight: 700; margin-bottom: 1rem; }
        .footer-logo span { color: #F59E0B; border: 1px solid #F59E0B; padding: 0 4px; border-radius: 4px; }
        .social-icons { display: flex; gap: 1rem; font-size: 1.5rem; margin-top: 1rem; }
        .footer h4 { margin-bottom: 1rem; font-weight: 600; }
        .footer ul { list-style: none; padding: 0; }
        .footer ul li { margin-bottom: 0.5rem; font-size: 0.9rem; color: #D1D5DB; cursor: pointer; }
        .newsletter-box {
          border: 1px solid var(--white);
          border-radius: 4px;
          display: flex;
          justify-content: space-between;
          align-items: center;
          padding: 0.8rem;
          margin-top: 1rem;
        }

        @media (max-width: 768px) {
          .profile-stats { grid-template-columns: 1fr; gap: 1rem; }
          .nav-links { display: none; }
          .footer-grid { grid-template-columns: 1fr; gap: 2rem; }
          .timeline-item { flex-direction: column; gap: 0.5rem; }
        }
      `}</style>

      <div className="profile-wrapper">
        <nav className="navbar">
          <div className="logo">KHADMA <span>HUB</span></div>
          <ul className="nav-links">
            <li><a href="#services">Services</a></li>
            <li><a href="#about">About</a></li>
            <li><a href="#jobs">Job</a></li>
          </ul>
          <div className="nav-actions">
            <i className="fa-regular fa-comment"></i>
            <i className="fa-regular fa-bell"></i>
            <span>AR</span>
            <div className="user-avatar-small">
              <i className="fa-regular fa-user"></i>
            </div>
          </div>
        </nav>

        <div className="cover-photo">
          <div className="cover-actions">
            <i className="fa-solid fa-trash"></i>
            <i className="fa-solid fa-pen"></i>
          </div>
        </div>

        <main className="profile-container">
          <div className="profile-header">
            <div className="profile-title-actions">
              <i className="fa-regular fa-share-from-square"></i>
            </div>

            <div className="avatar-container">
              <div className="profile-avatar"></div>
              <div className="avatar-actions">
                <i className="fa-solid fa-trash"></i>
                <i className="fa-solid fa-pen"></i>
              </div>
            </div>

            <h1 className="profile-name">Omnia Salah</h1>

            <div className="profile-stats">
              <div className="stat-item">
                <span>Member since 2025 Nov</span>
                <span>Cairo, Egypt <i className="fa-solid fa-location-dot"></i></span>
              </div>
              <div className="stat-item">
                <div className="stars">
                  <i className="fa-regular fa-star"></i>
                  <i className="fa-regular fa-star"></i>
                  <i className="fa-regular fa-star"></i>
                  <i className="fa-regular fa-star"></i>
                  <i className="fa-regular fa-star"></i> (0)
                </div>
                <span>Software engineer</span>
              </div>
              <div className="stat-item">
                <span>2 years experience</span>
                <span>Working 3 hours a week<br />as a freelancer</span>
              </div>
            </div>

            <button className="contact-btn">Contact me</button>
          </div>

          <div className="pricing-row">
            <span>Average per hour</span>
            <span>50 EG/HR</span>
          </div>

          <p className="bio-text">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et
            dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex
            ea commodo consequat.
          </p>

          <section>
            <div className="section-header">
              <div className="section-title"><i className="fa-solid fa-chevron-right"></i> Skills</div>
              <i className="fa-regular fa-pen-to-square action-icon"></i>
            </div>
            <div className="skills-list">
              <span className="skill-pill">Skills</span>
              <span className="skill-pill">Skills</span>
            </div>
          </section>

          <section>
            <div className="section-header">
              <div className="section-title"><i className="fa-solid fa-chevron-right"></i> Previous work</div>
              <div className="add-btn"><i className="fa-solid fa-plus"></i></div>
            </div>
            <div className="portfolio-grid">
              {[1, 2].map((i) => (
                <div key={i} className="portfolio-card">
                  <div className="portfolio-img" style={{ backgroundImage: "url('https://via.placeholder.com/400x200/222/555?text=Dashboard+Image')", backgroundSize: 'cover' }}></div>
                  <div className="portfolio-content">
                    <div className="portfolio-title-row">
                      <span>Name Work</span>
                      <i className="fa-regular fa-pen-to-square"></i>
                    </div>
                    <div className="skills-list" style={{ marginBottom: 0 }}>
                      <span className="skill-pill">UI</span>
                      <span className="skill-pill">UI</span>
                      <span className="skill-pill">UI</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </section>

          {/* Educational, Certification, Experience Sections */}
          {['Educational', 'certification', 'Experience'].map((title, idx) => (
            <section key={idx}>
              <div className="section-header">
                <div className="section-title"><i className="fa-solid fa-chevron-right"></i> {title}</div>
                <div className="add-btn"><i className="fa-solid fa-plus"></i></div>
              </div>
              <div className="timeline-item">
                <i className={`fa-solid ${title === 'Educational' ? 'fa-graduation-cap' : title === 'certification' ? 'fa-certificate' : 'fa-user-tie'} timeline-icon`}></i>
                <div className="timeline-content">
                  <div className="timeline-title-row">
                    <span className="timeline-title">"{title} Name"</span>
                    <i className="fa-regular fa-pen-to-square action-icon"></i>
                  </div>
                  {title === 'Educational' && <div className="timeline-subtitle">"specialty"</div>}
                  <p className="timeline-desc">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                  </p>
                  <div className="timeline-date">2022 /2/1 - 2026/5/1</div>
                </div>
              </div>
            </section>
          ))}
        </main>

        <footer className="footer">
          <div className="footer-grid">
            <div>
              <div className="footer-logo">KHADMA <span>HUB</span></div>
              <p style={{ fontSize: '0.9rem', marginBottom: '0.5rem' }}>Social media</p>
              <div className="social-icons">
                <i className="fa-brands fa-facebook" style={{ color: '#1877F2' }}></i>
                <i className="fa-brands fa-google" style={{ color: '#DB4437' }}></i>
                <i className="fa-brands fa-apple" style={{ color: 'white' }}></i>
              </div>
            </div>
            <div>
              <h4>About</h4>
              <ul>
                <li>About Us</li>
                <li>Why KHADMA HUB</li>
                <li>Reviews & Testimonials</li>
                <li>How KHADMA HUB work</li>
              </ul>
            </div>
            <div>
              <h4>Find jobs</h4>
              <ul>
                <li>development jobs</li>
                <li>Writing jobs</li>
                <li>Designers jobs</li>
                <li>Sales jobs</li>
              </ul>
            </div>
            <div>
              <h4>Sub-Heading</h4>
              <div className="newsletter-box">
                <span>Button Text</span>
                <i className="fa-solid fa-globe"></i>
              </div>
            </div>
          </div>
        </footer>
      </div>
    </>
  );
};

export default ProfilePage;