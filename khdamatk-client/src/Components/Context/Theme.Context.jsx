import { createContext, useState } from "react";

 export const ThemeContext=createContext();
 export default function ThemeProvider( {children}){
    const[theme,setTheme]=useState('light')
    function toggleTheme(){
        if(theme === "light"){
            setTheme('dark')
        }
        else{
            setTheme("light")
        }
    }
    return  <ThemeContext.Provider value={{ theme, setTheme , toggleTheme}}>
         {children}
    </ThemeContext.Provider>
}