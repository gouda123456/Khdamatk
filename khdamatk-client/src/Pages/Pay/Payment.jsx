import React, { useEffect } from 'react';

const FawaterkCheckout = () => {
  useEffect(() => {
    // 1. تعريف الإعدادات على كائن window مباشرة ليراها السكريبت الخارجي
    window.pluginConfig = {
      envType: "test",
      hashKey: "493e0a9b711f34731bc6375bf5404c33b2ead17a05b23830a5109ef577dc5b2a",
      style: { listing: "horizontal" },
      version: "0",
      requestBody: {
        cartTotal: "50",
        currency: "EGP",
        customer: {
          first_name: "test",
          last_name: "fawaterk",
          email: "test@fawaterk.com",
          phone: "0123456789",
          address: "test address"
        },
        redirectionUrls: {
          successUrl: "https://dev.fawaterk.com/success",
          failUrl: "https://dev.fawaterk.com/fail",
          pendingUrl: "https://dev.fawaterk.com/pending"
        },
        cartItems: [
          { name: "item 1", price: "25", quantity: "1" },
          { name: "item 2", price: "25", quantity: "1" }
        ],
        payLoad: {
          custom_field1: "order_123", // هنا ستضع معرّف الطلب من مشروعك لاحقاً
        }
      }
    };

    // 2. التحقق مما إذا كان السكريبت موجوداً مسبقاً لمنع خطأ 'initialState'
    const existingScript = document.getElementById('fawaterk-script');
    
    const initializeFawaterk = () => {
      if (window.fawaterkCheckout) {
        window.fawaterkCheckout(window.pluginConfig);
      }
    };

    if (!existingScript) {
      const script = document.createElement('script');
      script.id = 'fawaterk-script';
      script.src = 'https://app.fawaterk.com/fawaterkPlugin/fawaterkPlugin.min.js';
      script.async = true;
      script.onload = initializeFawaterk;
      document.body.appendChild(script);
    } else {
      // إذا كان السكريبت موجوداً بالفعل (بسبب التنقل بين الصفحات)
      initializeFawaterk();
    }

    // 3. التنظيف عند الخروج من الصفحة
    return () => {
        // لا نحذف السكريبت لتجنب أخطاء إعادة التعريف، فقط نفرغ الحاوية
        const div = document.getElementById('fawaterkDivId');
        if (div) div.innerHTML = '';
        // اختياري: حذف الإعدادات من window
        // delete window.pluginConfig; 
    };
  }, []);

  return (
    <div style={{ padding: '20px', minHeight: '400px' }}>
      <div id="fawaterkDivId"></div>
    </div>
  );
};

export default FawaterkCheckout;