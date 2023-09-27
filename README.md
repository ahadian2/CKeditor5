<h2>آموزش Asp.Net MVC آپلود تصویر در CKeditor 5</h2>
در این آموزش قصد داریم تا قابلیت آپلود تصویر در CKeditor 5 را پیکربندی و فعال کنیم. روش های مختلفی برای فعال سازی قابلیت آپلود عکس در <a href="http://tehranit.net/tag/ckeditor-5/">CKeditor 5</a> وجود دارد. یکی از امن ترین و سریع ترین روش ها استفاده از <a href="https://ckeditor.com/docs/ckeditor5/latest/framework/deep-dive/upload-adapter.html">Custom image upload adapter</a> میباشد که توسط سایت <a href="https://ckeditor.com/">CKeditor 5</a> معرفی شده است.

[caption id="attachment_10522" align="alignnone" width="1920"]<a href="http://tehranit.net/wp-content/uploads/2023/08/CkEditor5-Image-Upload.jpg"><img class="size-full wp-image-10522" src="http://tehranit.net/wp-content/uploads/2023/08/CkEditor5-Image-Upload.jpg" alt="Custom image upload adapter" width="1920" height="619" /></a> Custom image upload adapter[/caption]
<h2>گام اول : افزودن آداپتور - Adapter</h2>
<ul>
 	<li>در قسمتی که کد های Java Script ای خود را وارد میکنید، دقیقا بالای قسمتی که CKeditor خود را توسط دستورات جاوا اسکریپتی لود میکنید، کلاس و دستورات Custom image upload adapter را وارد کنید.</li>
</ul>
<blockquote>توجه در بخشی که به رنگ قرمز نوشته شده نام کنترلر و اکشنی که قرار است تصاویر را آپلود کند، وارد کنید.</blockquote>
<code>class MyUploadAdapter {
constructor(loader) {
// The file loader instance to use during the upload.
this.loader = loader;
}
// Starts the upload process.
upload() {
return this.loader.file
.then(file =&gt; new Promise((resolve, reject) =&gt; {
this._initRequest();
this._initListeners(resolve, reject, file);
this._sendRequest(file);
}));
}
// Aborts the upload process.
abort() {
if (this.xhr) {
this.xhr.abort();
}
}
// Initializes the XMLHttpRequest object using the URL passed to the constructor.
_initRequest() {
const xhr = this.xhr = new XMLHttpRequest();
// Note that your request may look different. It is up to you and your editor
// integration to choose the right communication channel. This example uses
// a POST request with JSON as a data structure but your configuration
// could be different.
xhr.open('POST', '<span style="color: #ff0000;">/Posts/MyUpload</span>', true);
xhr.responseType = 'json';
}
// Initializes XMLHttpRequest listeners.
_initListeners(resolve, reject, file) {
const xhr = this.xhr;
const loader = this.loader;
const genericErrorText = `Couldn't upload file: ${file.name}.`;
xhr.addEventListener('error', () =&gt; reject(genericErrorText));
xhr.addEventListener('abort', () =&gt; reject());
xhr.addEventListener('load', () =&gt; {
const response = xhr.response;
// This example assumes the XHR server's "response" object will come with
// an "error" which has its own "message" that can be passed to reject()
// in the upload promise.
//
// Your integration may handle upload errors in a different way so make sure
// it is done properly. The reject() function must be called when the upload fails.
if (!response || response.error) {
return reject(response &amp;&amp; response.error ? response.error.message : genericErrorText);
}
// If the upload is successful, resolve the upload promise with an object containing
// at least the "default" URL, pointing to the image on the server.
// This URL will be used to display the image in the content. Learn more in the
// UploadAdapter#upload documentation.
resolve({
default: response.url
});
});
// Upload progress when it is supported. The file loader has the #uploadTotal and #uploaded
// properties which are used e.g. to display the upload progress bar in the editor
// user interface.
if (xhr.upload) {
xhr.upload.addEventListener('progress', evt =&gt; {
if (evt.lengthComputable) {
loader.uploadTotal = evt.total;
loader.uploaded = evt.loaded;
}
});
}
}
// Prepares the data and sends the request.
_sendRequest(file) {
// Prepare the form data.
const data = new FormData();
data.append('upload', file);
// Important note: This is the right place to implement security mechanisms
// like authentication and CSRF protection. For instance, you can use
// XMLHttpRequest.setRequestHeader() to set the request headers containing
// the CSRF token generated earlier by your application.
// Send the request.
this.xhr.send(data);
}
}
function MyCustomUploadAdapterPlugin(editor) {
editor.plugins.get('FileRepository').createUploadAdapter = (loader) =&gt; {
// Configure the URL to the upload script in your back-end here!
return new MyUploadAdapter(loader);
};
}</code>
<ul>
 	<li>حال باید آداپتور - Adapter خود را در بحشی که مربوط به وارد کردن کانفیگ های CKeditor است، توسط کد زیر فراخوانی کنید.</li>
</ul>
<code>extraPlugins: [MyCustomUploadAdapterPlugin],</code>
<ul>
 	<li>در صورتی که با ارور روبرو شدید میتوانید CKeditor را مانند زیر فراخوانی کنید.</li>
</ul>
<code>window.addEventListener("load", (e) =&gt; {
ClassicEditor
.create(document.querySelector('.editor'), {
<span style="color: #ff0000;">extraPlugins: [MyCustomUploadAdapterPlugin],</span>
})
.then(editor =&gt; {
console.log(editor);
})
.catch(error =&gt; {
console.error(error);
});
});</code>
<h2>گام دوم : کدهای سمت سرور</h2>
<ul>
 	<li>ابتدا یک کنترلر و یک اکشن ایجاد میکنیم، در مرحله قبل مشخص کردیم اطلاعات به کنترلر Posts و اکشن MyUpload ارسال شود. همچنین توجه داشته باشید که اکشن ما باید دارای اتربیوت HttpPost باشد.</li>
</ul>
<code>        [HttpPost]
public ActionResult MyUpload()
{
}</code>
<ul>
 	<li>برای دسترسی به فایل های ارسال شده میتوان از دستور Request.Files استفاده کرد. پس ابتدا توسط شرطی برسی میکنیم ببینیم فایلی ارسال شده یا خیر. در صورتی که فایلی ارسال نشده باشد به بخش else منتقل میشویم و در انجا توسط یک مقدار Json مقدار error را به سمت کلاینت ارسال میکنیم.</li>
</ul>
<code>            if (Request.Files.Count &gt; 0)
{
}
else
{
return Json(new { uploaded = false, error = "تصویری برای آپلود ارسال نشده است", JsonRequestBehavior.AllowGet });
}</code>
<ul>
 	<li>اگر فایل ارسال شده باشد وارد شرط میشویم. ابتدا باید یک ساختار try catch ایجاد کنیم. تا  در صورتی که با ارور روبرو شدیم آن را مدیریت کنیم. در صورتی که Error یا مشکلی پیش اید به بخش catch منتقل میشویم و در انجا توسط یک مقدار Json مقدار error را به سمت کلاینت ارسال میکنیم.</li>
</ul>
<code>                try
{
}
catch (Exception ex)
{
return Json(new { uploaded = false, error = "خطایی روی داده است" + ex.Message, JsonRequestBehavior.AllowGet });
}</code>
<ul>
 	<li>و در نهایت عکس های ارسال شده را دریافت و توسط یک حلقه یکی یکی در سرور ذحیره میکنیم.</li>
</ul>
Loaction مسیر فولدری میباشد که فایل ها در آن ذخیره میشوند، بهتر است این مسیر ایجاد شود.

NewName نام جدیدی است که برای تصویر به صورت یکتا ایجاد میشود تا از نان تکراری جلوگیری شود.

extension فرمت فایل یا تصویر را میگیرد مثلا فرمت .jpg

Imagename نام جدید تصویر همراه با قرمت آن میباشد.

ImageUrl آدرس تصویر از مبدا میباشد از این مقدار برای ذخیره در دیتابیس و یا آدرس دهی به تگ Img استفاده میشود.

ImageUrlUpload از این آدرس برای آپلود تصویر استفاده میشود.

<code>HttpFileCollectionBase files = Request.Files;
for (int i = 0; i &lt; files.Count; i++)
{
HttpPostedFileBase file = files[i];
string Loaction = "/Images/upload";
string NewName = Guid.NewGuid().ToString();
string extension = Path.GetExtension(file.FileName);
string Imagename = NewName + extension;
string ImageUrl = Loaction + "/" + Imagename;
string ImageUrlUpload = Path.Combine(Server.MapPath(Loaction), Imagename);
file.SaveAs(ImageUrlUpload);
return Json(new { uploaded = true, url = ImageUrl, JsonRequestBehavior.AllowGet });
}
return Json(new { uploaded = true, JsonRequestBehavior.AllowGet });</code>
<ul>
 	<li>مشاهده کد های اکشن به صورت یکجا</li>
</ul>
<code>        [HttpPost]
public ActionResult MyUpload()
{
if (Request.Files.Count &gt; 0)
{
try
{
HttpFileCollectionBase files = Request.Files;
for (int i = 0; i &lt; files.Count; i++)
{
HttpPostedFileBase file = files[i];
string Loaction = "/Images/upload";
string NewName = Guid.NewGuid().ToString();
string extension = Path.GetExtension(file.FileName);
string Imagename = NewName + extension;
string ImageUrl = Loaction + "/" + Imagename;
string ImageUrlUpload = Path.Combine(Server.MapPath(Loaction), Imagename);
file.SaveAs(ImageUrlUpload);
return Json(new { uploaded = true, url = ImageUrl, JsonRequestBehavior.AllowGet });
}
return Json(new { uploaded = true, JsonRequestBehavior.AllowGet });
}
catch (Exception ex)
{
return Json(new { uploaded = false, error = "خطایی روی داده است" + ex.Message, JsonRequestBehavior.AllowGet });
}
}
else
{
return Json(new { uploaded = false, error = "تصویری برای آپلود ارسال نشده است", JsonRequestBehavior.AllowGet });
}
}</code>
<h2>نقاط ضعف Insert Image یا همان آپلود تصویر CKeditor 5</h2>
<ul>
 	<li>درصورتی که به تصویر نیازی نداشته باشیم نمیتوانیم آن را از سرور حذف کنیم و این باعث افزونگی در سرور میشود.</li>
 	<li>در صورتی که بخواهیم از یک تصویر در ۲ بهش استفاده کنیم باید آن تصویر را ۲ بار در سرور آپلود کنیم. که این هم باعث افزونگی در سرور میشود.</li>
</ul>
البته برای رفع این مشکل CKeditor راه کار هایی مثل CKbox و CKfinder را ایجاد کرده اما متاسفانه رایگان نمیباشند. و برای رفع این مشکل در قسمت بعدی این آموزش یک مدیریت فایل را ایجاد و به ادیتور خود اضافه میکنیم.
<h2>ویدیو آموزش آپلود تصویر در CKeditor 5</h2>
    <script type="text/JavaScript" src="https://www.aparat.com/embed/zmfD7?data[rnddiv]=85641651399&data[responsive]=yes"></script>
