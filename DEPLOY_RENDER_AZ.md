# SilentInterview — Render deploy təlimatı (AZ)

Bu layihə Render üçün `render.yaml` ilə qurulub. GitHub-a yüklənən kodun içində production `.env` və parol yoxdur. DB parolunu və JWT açarını Render özü yaradır.

## 1. GitHub-a yüklə
1. ZIP-i aç.
2. İçindəki `SilentIntervieww` qovluğunun **içindəkiləri** yeni, boş GitHub repository-yə yüklə.
3. Repository-nin ən üst səviyyəsində `render.yaml` görünməlidir.
4. Bu ZIP-də `.git` qovluğu qəsdən yoxdur. Əvvəlki Git tarixçəsində sirr saxlanmış ola bilər; təmiz repository yaratmaq daha təhlükəsizdir.

## 2. Render-dən deploy et
1. Render Dashboard-a daxil ol.
2. **New → Blueprint** seç.
3. Yeni yaratdığın GitHub repository-ni qoş.
4. Render `render.yaml` faylını tapmalıdır.
5. İlk qurulumda `SUPERADMIN_EMAIL` üçün öz istifadə edəcəyin e-poçt ünvanını daxil et. `SUPERADMIN_PASSWORD`, `Jwt__Key` və PostgreSQL bağlantı məlumatları Render tərəfindən avtomatik yaradılır.
6. Blueprint resurslarını yarat və deploy et. API xidməti, PostgreSQL bazası və fayl diski ödənişli planlardır; təsdiqdən əvvəl Render-də qiymətləri yoxla.
7. Deploy bitəndə API yoxlaması üçün `https://silentinterview-prod-api-v2.onrender.com/health/ready` ünvanını aç. `status: ready` və `database: ok` görünməlidir.

## 3. Sayt ünvanı
Standart ünvanlar Blueprint-də belədir:
- Frontend: `https://silentinterview-prod-web-v2.onrender.com`
- API: `https://silentinterview-prod-api-v2.onrender.com`

Əgər Render hansısa service adının artıq istifadə olunduğunu bildirərsə, `render.yaml` içində həmin service adını dəyiş. Frontend domeni dəyişərsə, API-dəki `FRONTEND_ORIGIN` və `Frontend__BaseUrl` dəyərlərini də yeni frontend URL-ə uyğunlaşdır; `VITE_API_BASE_URL` isə API URL-inə uyğun olmalıdır.

## 4. İlk admin hesabı
Render API service → **Environment** bölməsində `SUPERADMIN_PASSWORD` dəyərini göstər. İlk giriş üçün e-poçt olaraq deploy wizard-da daxil etdiyin `SUPERADMIN_EMAIL`-i, parol olaraq isə həmin avtomatik yaradılmış `SUPERADMIN_PASSWORD`-u istifadə et. Parolu sonradan dəyiş.

## 5. Əlavə inteqrasiyalar
Bunlar üçün müvafiq xidmətdən real açar almalısan və yalnız Render → API service → Environment bölməsinə əlavə etməlisən:

| Funksiya | Render environment key-ləri |
|---|---|
| AI müsahibə və hesabatlar | `GROQ_API_KEY` |
| Səsin mətnə çevrilməsi | `OpenAI__ApiKey` |
| Google ilə giriş | `Google__ClientId` |
| Stripe ödənişləri | `Stripe__SecretKey`, `Stripe__WebhookSecret`, `Stripe__GoPriceId`, `Stripe__ProPriceId` |
| Şifrə sıfırlama e-poçtu | `Smtp__Host`, `Smtp__Port`, `Smtp__From`, `Smtp__Username`, `Smtp__Password` |

Bu açarları uydurmaq mümkün deyil. Onlar olmadan müvafiq üçüncü tərəf inteqrasiyası aktiv olmayacaq, amma əsas email/parol qeydiyyatı və giriş üçün bu açarlar tələb olunmur.

Stripe webhook URL: `https://<SENIN-API-SERVICE-URL>/api/v1/subscription/webhook` — `<SENIN-API-SERVICE-URL>` hissəsini Render-də API servisinin göstərdiyi real URL ilə əvəz et.
Google OAuth üçün frontend domenini Google Cloud Console-da Authorized JavaScript origins siyahısına əlavə et.

## 6. DB və fayl saxlanması
- Yeni PostgreSQL bazası 30 günlük Free DB müddətinə düşməyən ödənişli plandadır.
- Şirkət loqoları `/app/wwwroot/uploads` mount edilmiş persistent disk-də saxlanır.
- Yeni DB boş yaradılır; əvvəlki expired DB-dəki məlumatlar avtomatik köçürülmür.
- DB schema API startup zamanı yaradılır. DB əlçatmazdırsa API startup uğursuz olacaq, hazır kimi göstərilməyəcək.
- Render HTTPS proxy başlıqları aktivdir; API özünü HTTPS sorğusu kimi tanıyır və Secure cookie-lər düzgün işləyir.

## 7. Təhlükəsizlik
- `.env` fayllarını GitHub-a yükləmə.
- Əvvəlki `.env` və Git tarixçəsində istifadə edilmiş DB parolu/JWT/API açarlarını yenilə və ya revoke et.
- Production sirlərini yalnız Render Environment bölməsində saxla.
- Render qiymətlərini deploy etməzdən əvvəl yoxla: API, DB və disk ödənişlidir.
