 export function checkPasswordStrenght(password){
    let strenght= 0;
    let feedBack= {text:"", background:"",width:""};
    if(password.length > 8) strenght++;
    if(password.length > 12) strenght++;
    if(/[a-z]/.test(password))strenght++;
    if(/[A-Z]/.test(password))strenght++;
    if(/[0-9]/.test(password))strenght++;
    if(/[!@#$%^&*()]/.test(password))strenght++;

    switch(strenght){
        case 1:
            feedBack.background="bg-red-500";
            feedBack.text="Very Week";
            feedBack.width="w-1/6";
            break;

              case 2:
            feedBack.background="bg-orange-600";
            feedBack.text="Week";
            feedBack.width="w-2/6";
            break;

              case 3:
            feedBack.background="bg-yellow-600";
            feedBack.text="Fair";
            feedBack.width="w-3/6";
            break;

              case 4:
            feedBack.background="bg-lime-600";
            feedBack.text="Good";
            feedBack.width="w-4/6";
            break;

              case 5:
            feedBack.background="bg-primary-600";
            feedBack.text="Strong";
            feedBack.width="w-5/6";
            break;

              case 6:
            feedBack.background="bg-primary-900";
            feedBack.text="Very Strong";
            feedBack.width="w-6/6";
            break;


    }
    return feedBack;


}