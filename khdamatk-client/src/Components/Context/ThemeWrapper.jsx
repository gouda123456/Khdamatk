import { useContext } from 'react';
import { ThemeContext } from './Theme.Context'; // ✅ صح

export default function ThemeWrapper({ children }) {
  const { theme } = useContext(ThemeContext);

  return (
    <div className={theme === 'dark' ? 'dark' : ''}>
      {children}
    </div>
  );
}