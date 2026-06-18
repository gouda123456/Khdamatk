
using Microsoft.AspNetCore.Http;

namespace Khdamatk.Server.Contracts.Admin.Disputes;

// 1. الداتا اللي هترجع للشاشة عشان تتعرض بالكامل
public class FinalDecisionResponse
{
    public string ReportId { get; set; } = string.Empty; // RPT-2026-001547
    public string Type { get; set; } = string.Empty; // Account / Service Issue
    public string ClientName { get; set; } = string.Empty; // Ahmed Mohamed
    public string FreelancerName { get; set; } = string.Empty; // Omar Hassan
    public string ResolvedBy { get; set; } = string.Empty; // Senior Admin
    public DateTime SubmittedDate { get; set; } // May 26, 2026
    public DateTime? ReviewedDate { get; set; } // May 27, 2026
    public string ResolutionTime { get; set; } = string.Empty; // 1 day 4 hours

    // بيانات كارت التعويض والقرار
    public string ClaimStatus { get; set; } = string.Empty; // CLAIM APPROVED
    public decimal CompensationAmount { get; set; } // 2450.00
    public string CompensationType { get; set; } = string.Empty; // Refund to Client
    public string DecisionNotes { get; set; } = string.Empty; // نص القرار
    public List<string> EvidenceUrls { get; set; } = new(); // روابط الصور المرفقة بالقرار إن وجدت
}

// 2. الداتا اللي هتوصل من الأدمن لما يضغط على اتخاذ القرار ويرفع صور
public class SubmitDecisionRequest
{
    public int DisputeId { get; set; } // الـ ID بتاع النزاع في الداتابيز
    public bool IsApproved { get; set; } // true للـ Approve و false للـ Reject
    public string DecisionNotes { get; set; } = string.Empty; // ملاحظات القرار
    public decimal CompensationAmount { get; set; } // مبلغ التعويض لو الأدمن عدله

    // 📸 استقبال صور أو ملفات إثبات زي الكود بتاعكم في المشروع
    public List<IFormFile>? Attachments { get; set; }
}