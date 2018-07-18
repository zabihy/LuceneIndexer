﻿using LuceneEngine.Models.CacheModels;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Fa;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Support;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LuceneEngine.Core
{
    public class AnalyzerFactory
    {
        public Analyzer Create<TEntity>()
        {
            //return new WhitespaceAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            //return new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            //return new CustomAnalyzer();

            var type = typeof(TEntity);

            if (type == typeof(ArticleCacheEntity))
            {
                return GenerateArticleAnalyzer();
            }

            else if (type == typeof(AuthorCacheEntity))
            {
                return GenerateAuthorAnalyzer();
            }
            else
            {
                throw new Exception($"There is no analyzer for type {typeof(TEntity).Name}");
            }
        }

        public class CustomAnalyzer : Analyzer
        {
            private CharArraySet _defaultStopwords;

            private CharArraySet _defaultArticles;
            public CustomAnalyzer()
            {
                _defaultStopwords = new PersianAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48).StopwordSet;

                _defaultArticles = CharArraySet.UnmodifiableSet(
                   new CharArraySet(Lucene.Net.Util.LuceneVersion.LUCENE_48,
                       Arrays.AsList(
                          Stopwords
                       ), true));
            }

            protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
            {
                Tokenizer source = new StandardTokenizer(Lucene.Net.Util.LuceneVersion.LUCENE_48, reader);
                TokenStream result = new StandardFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, source);
                result = new ElisionFilter(result, _defaultArticles);
                result = new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, result);
                result = new StopFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, result, _defaultStopwords);
                result = new PermutermTokenFilter(result);
                return new TokenStreamComponents(source, result);
            }

            private static string [] Stopwords= new string[] { "!", ",", ".", ":", ";", "،", "؛", "؟", "آباد", "آره", "آری", "آمد", "آمده", "آن", "آنان", "آنجا", "آنطور", "آنقدر", "آنكه", "آنها", "آنچه", "آنکه", "آورد", "آورده", "آيد", "آی", "آیا", "آیند", "اتفاقا", "اثرِ", "احتراما", "احتمالا", "اخیر", "اری", "از", "ازجمله", "اساسا", "است", "استفاد", "استفاده", "اش", "اشکارا", "اصلا", "اصولا", "اعلام", "اغلب", "اكنون", "الان", "البته", "البتّه", "ام", "اما", "امروز", "امروزه", "امسال", "امشب", "امور", "ان", "انجام", "اند", "انشاالله", "انصافا", "انطور", "انقدر", "انها", "انچنان", "انکه", "انگار", "او", "اول", "اولا", "اي", "ايشان", "ايم", "اين", "اينكه", "اکثرا", "اکنون", "اگر", "ای", "ایا", "اید", "ایشان", "ایم", "این", "اینجا", "ایند", "اینطور", "اینقدر", "اینها", "اینچنین", "اینک", "اینکه", "اینگونه", "با", "بار", "بارة", "باره", "بارها", "باز", "بازهم", "باش", "باشد", "باشم", "باشند", "باشيم", "باشی", "باشید", "باشیم", "بالا", "بالاخره", "بالایِ", "بالطبع", "بايد", "باید", "بتوان", "بتواند", "بتوانی", "بتوانیم", "بخش", "بخشی", "بخواه", "بخواهد", "بخواهم", "بخواهند", "بخواهی", "بخواهید", "بخواهیم", "بد", "بدون", "بر", "برابر", "برابرِ", "براحتی", "براساس", "براستی", "براي", "برای", "برایِ", "برخوردار", "برخي", "برخی", "برداري", "برعکس", "بروز", "بزرگ", "بزودی", "بسا", "بسيار", "بسياري", "بسیار", "بسیاری", "بطور", "بعد", "بعدا", "بعدها", "بعری", "بعضا", "بعضي", "بلافاصله", "بلكه", "بله", "بلکه", "بلی", "بنابراين", "بنابراین", "بندي", "به", "بهتر", "بهترين", "بود", "بودم", "بودن", "بودند", "بوده", "بودی", "بودید", "بودیم", "بویژه", "بي", "بيست", "بيش", "بيشتر", "بيشتري", "بين", "بکن", "بکند", "بکنم", "بکنند", "بکنی", "بکنید", "بکنیم", "بگو", "بگوید", "بگویم", "بگویند", "بگویی", "بگویید", "بگوییم", "بگیر", "بگیرد", "بگیرم", "بگیرند", "بگیری", "بگیرید", "بگیریم", "بی", "بیا", "بیاب", "بیابد", "بیابم", "بیابند", "بیابی", "بیابید", "بیابیم", "بیاور", "بیاورد", "بیاورم", "بیاورند", "بیاوری", "بیاورید", "بیاوریم", "بیاید", "بیایم", "بیایند", "بیایی", "بیایید", "بیاییم", "بیرون", "بیرونِ", "بیش", "بیشتر", "بیشتری", "بین", "ت", "تا", "تازه", "تاكنون", "تان", "تاکنون", "تحت", "تر", "تر  براساس", "ترين", "تقریبا", "تلویحا", "تمام", "تماما", "تمامي", "تنها", "تو", "تواند", "توانست", "توانستم", "توانستن", "توانستند", "توانسته", "توانستی", "توانستیم", "توانم", "توانند", "توانی", "توانید", "توانیم", "توسط", "تولِ", "تویِ", "ثانیا", "جا", "جاي", "جايي", "جای", "جدا", "جديد", "جدید", "جريان", "جریان", "جز", "جلوگيري", "جلویِ", "جمعا", "جناح", "جهت", "حاضر", "حال", "حالا", "حتما", "حتي", "حتی", "حداکثر", "حدودا", "حدودِ", "حق", "خارجِ", "خب", "خدمات", "خصوصا", "خلاصه", "خواست", "خواستم", "خواستن", "خواستند", "خواسته", "خواستی", "خواستید", "خواستیم", "خواهد", "خواهم", "خواهند", "خواهيم", "خواهی", "خواهید", "خواهیم", "خوب", "خود", "خودت", "خودتان", "خودش", "خودشان", "خودم", "خودمان", "خوشبختانه", "خويش", "خویش", "خویشتن", "خیاه", "خیر", "خیلی", "داد", "دادم", "دادن", "دادند", "داده", "دادی", "دادید", "دادیم", "دار", "دارد", "دارم", "دارند", "داريم", "داری", "دارید", "داریم", "داشت", "داشتم", "داشتن", "داشتند", "داشته", "داشتی", "داشتید", "داشتیم", "دانست", "دانند", "دایم", "دایما", "در", "درباره", "درمجموع", "درون", "دریغ", "دقیقا", "دنبالِ", "ده", "دهد", "دهم", "دهند", "دهی", "دهید", "دهیم", "دو", "دوباره", "دوم", "ديده", "ديروز", "ديگر", "ديگران", "ديگري", "دیر", "دیروز", "دیگر", "دیگران", "دیگری", "را", "راحت", "راسا", "راستی", "راه", "رسما", "رسید", "رفت", "رفته", "رو", "روب", "روز", "روزانه", "روزهاي", "روي", "روی", "رویِ", "ريزي", "زمان", "زمانی", "زمینه", "زود", "زياد", "زير", "زيرا", "زیر", "زیرِ", "سابق", "ساخته", "سازي", "سالانه", "سالیانه", "سایر", "سراسر", "سرانجام", "سریعا", "سریِ", "سعي", "سمتِ", "سوم", "سوي", "سوی", "سویِ", "سپس", "شان", "شايد", "شاید", "شخصا", "شد", "شدم", "شدن", "شدند", "شده", "شدی", "شدید", "شدیدا", "شدیم", "شش", "شش  نداشته", "شما", "شناسي", "شود", "شوم", "شوند", "شونده", "شوی", "شوید", "شویم", "صرفا", "صورت", "ضدِّ", "ضدِّ", "ضمن", "طبعا", "طبقِ", "طبیعتا", "طرف", "طريق", "طریق", "طور", "طي", "طی", "ظاهرا", "عدم", "عقبِ", "علّتِ", "علیه", "عمدا", "عمدتا", "عمل", "عملا", "عنوان", "عنوانِ", "غالبا", "غير", "غیر", "فردا", "فعلا", "فقط", "فكر", "فوق", "قابل", "قبل", "قبلا", "قدری", "قصدِ", "قطعا", "كرد", "كردم", "كردن", "كردند", "كرده", "كسي", "كل", "كمتر", "كند", "كنم", "كنند", "كنيد", "كنيم", "كه", "لااقل", "لطفا", "لطفاً", "ما", "مان", "مانند", "مانندِ", "مبادا", "متاسفانه", "متعاقبا", "مثل", "مثلا", "مثلِ", "مجانی", "مجددا", "مجموعا", "مختلف", "مدام", "مدت", "مدّتی", "مردم", "مرسی", "مستقیما", "مسلما", "مطمینا", "معمولا", "مقابل", "ممکن", "من", "موارد", "مورد", "موقتا", "مي", "ميليارد", "ميليون", "مگر", "می", "می شود", "میان", "می‌رسد", "می‌رود", "می‌شود", "می‌کنیم", "ناشي", "نام", "ناگاه", "ناگهان", "ناگهانی", "نبايد", "نباید", "نبود", "نخست", "نخستين", "نخواهد", "نخواهم", "نخواهند", "نخواهی", "نخواهید", "نخواهیم", "ندارد", "ندارم", "ندارند", "نداری", "ندارید", "نداریم", "نداشت", "نداشتم", "نداشتند", "نداشته", "نداشتی", "نداشتید", "نداشتیم", "نزديك", "نزدِ", "نزدیکِ", "نسبتا", "نشان", "نشده", "نظير", "نظیر", "نكرده", "نمايد", "نمي", "نمی", "نمی‌شود", "نه", "نهایتا", "نوع", "نوعي", "نوعی", "نيز", "نيست", "نگاه", "نیز", "نیست", "ها", "هاي", "هايي", "های", "هایی", "هبچ", "هر", "هرچه", "هرگز", "هزار", "هست", "هستم", "هستند", "هستيم", "هستی", "هستید", "هستیم", "هفت", "هم", "همان", "همه", "همواره", "همين", "همچنان", "همچنين", "همچنین", "همچون", "همیشه", "همین", "هنوز", "هنگام", "هنگامِ", "هنگامی", "هيچ", "هیچ", "هیچگاه", "و", "واقعا", "واقعی", "وجود", "وسطِ", "وضع", "وقتي", "وقتی", "وقتیکه", "ولی", "وي", "وگو", "وی", "ویژه", "يا", "يابد", "يك", "يكديگر", "يكي", "ّه", "٪", "پارسال", "پاعینِ", "پس", "پنج", "پيش", "پیدا", "پیش", "پیشاپیش", "پیشتر", "پیشِ", "چرا", "چطور", "چقدر", "چنان", "چنانچه", "چنانکه", "چند", "چندین", "چنين", "چنین", "چه", "چهار", "چو", "چون", "چيزي", "چگونه", "چیز", "چیزی", "چیست", "کاش", "کامل", "کاملا", "کتبا", "کجا", "کجاست", "کدام", "کرد", "کردم", "کردن", "کردند", "کرده", "کردی", "کردید", "کردیم", "کس", "کسانی", "کسی", "کل", "کلا", "کم", "کماکان", "کمتر", "کمتری", "کمی", "کن", "کنار", "کنارِ", "کند", "کنم", "کنند", "کننده", "کنون", "کنونی", "کنی", "کنید", "کنیم", "که", "کو", "کَی", "کی", "گاه", "گاهی", "گذاري", "گذاشته", "گذشته", "گردد", "گرفت", "گرفتم", "گرفتن", "گرفتند", "گرفته", "گرفتی", "گرفتید", "گرفتیم", "گروهي", "گفت", "گفتم", "گفتن", "گفتند", "گفته", "گفتی", "گفتید", "گفتیم", "گه", "گهگاه", "گو", "گويد", "گويند", "گویا", "گوید", "گویم", "گویند", "گویی", "گویید", "گوییم", "گيرد", "گيري", "گیرد", "گیرم", "گیرند", "گیری", "گیرید", "گیریم", "ی", "یا", "یابد", "یابم", "یابند", "یابی", "یابید", "یابیم", "یافت", "یافتم", "یافتن", "یافته", "یافتی", "یافتید", "یافتیم", "یعنی", "یقینا", "یه", "یک", "یکی", "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        }

        private Analyzer GenerateAuthorAnalyzer()
        {
            IDictionary<String, Analyzer> analyzerPerField = new Dictionary<string, Analyzer>();

            AuthorCacheEntity entity = new AuthorCacheEntity();
            analyzerPerField.Add(nameof(entity.AuthorId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.SourceId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.IsDeleted).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.University).ToLower(), new KeywordAnalyzer());

            PerFieldAnalyzerWrapper aWrapper =
              new PerFieldAnalyzerWrapper(new CustomAnalyzer(), analyzerPerField);

            return aWrapper;
        }

        private Analyzer GenerateArticleAnalyzer()
        {
            IDictionary<String, Analyzer> analyzerPerField = new Dictionary<string, Analyzer>();

            var entity = new ArticleCacheEntity();

            analyzerPerField.Add(nameof(entity.AdvisorId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.AllAuthorIds).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.ArticleId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.ArticleTypeId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.AuthorId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.ClassId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.CrawlHistoryId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.GrandParentSourceId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.MainArticleTypeId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.ParentSourceId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.SourceId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.SupervisorId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.IsArticle).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.IsDeleted).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.AuthorName).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.University).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.MagazineName).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.MagazineId).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.Keyword).ToLower(), new KeywordAnalyzer());
            analyzerPerField.Add(nameof(entity.ClassIsDeleted).ToLower(), new KeywordAnalyzer());

            PerFieldAnalyzerWrapper aWrapper =
              new PerFieldAnalyzerWrapper(new CustomAnalyzer(), analyzerPerField);

            return aWrapper;
        }
    }

    public class NewAnalyzerFactory
    {
        private static IDictionary<string, IEnumerable<string>> KeywordFields;

        public static IDictionary<string, Analyzer> Analyzers;
        public NewAnalyzerFactory()
        {
            KeywordFields = new Dictionary<string, IEnumerable<string>>();

            Analyzers = new Dictionary<string, Analyzer>();

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(BaseCacheEntity));

            foreach (var item in types)
            {
                var fields = item.GetFields().Where(f => f.CustomAttributes.Any(ca => ca.AttributeType == typeof(KeywordAttribute))).Select(f => f.Name.ToLower());

                KeywordFields.Add(item.Name.ToLower(), fields);
            }

            foreach (var item in types)
            {
                Analyzers.Add(item.Name.ToLower(), GenerateAnalyzer(item));
            }
        }

        public Analyzer Create<TEntity>()
        {
            return Analyzers.FirstOrDefault(p => p.Key == typeof(TEntity).Name.ToLower()).Value;
        }
        private Analyzer GenerateAnalyzer(Type type)
        {
            IDictionary<String, Analyzer> analyzerPerField = new Dictionary<string, Analyzer>();

            var propNames = KeywordFields.FirstOrDefault(p => p.Key == type.Name.ToLower()).Value;

            foreach (var item in propNames)
            {
                analyzerPerField.Add(item, new KeywordAnalyzer());
            }

            PerFieldAnalyzerWrapper aWrapper =
              new PerFieldAnalyzerWrapper(new CustomAnalyzer(), analyzerPerField);

            return aWrapper;
        }

        public class CustomAnalyzer : Analyzer
        {
            private CharArraySet _defaultStopwords;

            private CharArraySet _defaultArticles;
            public CustomAnalyzer()
            {
                _defaultStopwords = new PersianAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48).StopwordSet;

                _defaultArticles = CharArraySet.UnmodifiableSet(
                   new CharArraySet(Lucene.Net.Util.LuceneVersion.LUCENE_48,
                       Arrays.AsList(
                          Stopwords
                       ), true));
            }

            protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
            {
                Tokenizer source = new StandardTokenizer(Lucene.Net.Util.LuceneVersion.LUCENE_48, reader);
                TokenStream result = new StandardFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, source);
                result = new ElisionFilter(result, _defaultArticles);
                result = new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, result);
                result = new StopFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, result, _defaultStopwords);
                result = new PermutermTokenFilter(result);
                return new TokenStreamComponents(source, result);
            }

            private static string[] Stopwords = new string[] { "!", ",", ".", ":", ";", "،", "؛", "؟", "آباد", "آره", "آری", "آمد", "آمده", "آن", "آنان", "آنجا", "آنطور", "آنقدر", "آنكه", "آنها", "آنچه", "آنکه", "آورد", "آورده", "آيد", "آی", "آیا", "آیند", "اتفاقا", "اثرِ", "احتراما", "احتمالا", "اخیر", "اری", "از", "ازجمله", "اساسا", "است", "استفاد", "استفاده", "اش", "اشکارا", "اصلا", "اصولا", "اعلام", "اغلب", "اكنون", "الان", "البته", "البتّه", "ام", "اما", "امروز", "امروزه", "امسال", "امشب", "امور", "ان", "انجام", "اند", "انشاالله", "انصافا", "انطور", "انقدر", "انها", "انچنان", "انکه", "انگار", "او", "اول", "اولا", "اي", "ايشان", "ايم", "اين", "اينكه", "اکثرا", "اکنون", "اگر", "ای", "ایا", "اید", "ایشان", "ایم", "این", "اینجا", "ایند", "اینطور", "اینقدر", "اینها", "اینچنین", "اینک", "اینکه", "اینگونه", "با", "بار", "بارة", "باره", "بارها", "باز", "بازهم", "باش", "باشد", "باشم", "باشند", "باشيم", "باشی", "باشید", "باشیم", "بالا", "بالاخره", "بالایِ", "بالطبع", "بايد", "باید", "بتوان", "بتواند", "بتوانی", "بتوانیم", "بخش", "بخشی", "بخواه", "بخواهد", "بخواهم", "بخواهند", "بخواهی", "بخواهید", "بخواهیم", "بد", "بدون", "بر", "برابر", "برابرِ", "براحتی", "براساس", "براستی", "براي", "برای", "برایِ", "برخوردار", "برخي", "برخی", "برداري", "برعکس", "بروز", "بزرگ", "بزودی", "بسا", "بسيار", "بسياري", "بسیار", "بسیاری", "بطور", "بعد", "بعدا", "بعدها", "بعری", "بعضا", "بعضي", "بلافاصله", "بلكه", "بله", "بلکه", "بلی", "بنابراين", "بنابراین", "بندي", "به", "بهتر", "بهترين", "بود", "بودم", "بودن", "بودند", "بوده", "بودی", "بودید", "بودیم", "بویژه", "بي", "بيست", "بيش", "بيشتر", "بيشتري", "بين", "بکن", "بکند", "بکنم", "بکنند", "بکنی", "بکنید", "بکنیم", "بگو", "بگوید", "بگویم", "بگویند", "بگویی", "بگویید", "بگوییم", "بگیر", "بگیرد", "بگیرم", "بگیرند", "بگیری", "بگیرید", "بگیریم", "بی", "بیا", "بیاب", "بیابد", "بیابم", "بیابند", "بیابی", "بیابید", "بیابیم", "بیاور", "بیاورد", "بیاورم", "بیاورند", "بیاوری", "بیاورید", "بیاوریم", "بیاید", "بیایم", "بیایند", "بیایی", "بیایید", "بیاییم", "بیرون", "بیرونِ", "بیش", "بیشتر", "بیشتری", "بین", "ت", "تا", "تازه", "تاكنون", "تان", "تاکنون", "تحت", "تر", "تر  براساس", "ترين", "تقریبا", "تلویحا", "تمام", "تماما", "تمامي", "تنها", "تو", "تواند", "توانست", "توانستم", "توانستن", "توانستند", "توانسته", "توانستی", "توانستیم", "توانم", "توانند", "توانی", "توانید", "توانیم", "توسط", "تولِ", "تویِ", "ثانیا", "جا", "جاي", "جايي", "جای", "جدا", "جديد", "جدید", "جريان", "جریان", "جز", "جلوگيري", "جلویِ", "جمعا", "جناح", "جهت", "حاضر", "حال", "حالا", "حتما", "حتي", "حتی", "حداکثر", "حدودا", "حدودِ", "حق", "خارجِ", "خب", "خدمات", "خصوصا", "خلاصه", "خواست", "خواستم", "خواستن", "خواستند", "خواسته", "خواستی", "خواستید", "خواستیم", "خواهد", "خواهم", "خواهند", "خواهيم", "خواهی", "خواهید", "خواهیم", "خوب", "خود", "خودت", "خودتان", "خودش", "خودشان", "خودم", "خودمان", "خوشبختانه", "خويش", "خویش", "خویشتن", "خیاه", "خیر", "خیلی", "داد", "دادم", "دادن", "دادند", "داده", "دادی", "دادید", "دادیم", "دار", "دارد", "دارم", "دارند", "داريم", "داری", "دارید", "داریم", "داشت", "داشتم", "داشتن", "داشتند", "داشته", "داشتی", "داشتید", "داشتیم", "دانست", "دانند", "دایم", "دایما", "در", "درباره", "درمجموع", "درون", "دریغ", "دقیقا", "دنبالِ", "ده", "دهد", "دهم", "دهند", "دهی", "دهید", "دهیم", "دو", "دوباره", "دوم", "ديده", "ديروز", "ديگر", "ديگران", "ديگري", "دیر", "دیروز", "دیگر", "دیگران", "دیگری", "را", "راحت", "راسا", "راستی", "راه", "رسما", "رسید", "رفت", "رفته", "رو", "روب", "روز", "روزانه", "روزهاي", "روي", "روی", "رویِ", "ريزي", "زمان", "زمانی", "زمینه", "زود", "زياد", "زير", "زيرا", "زیر", "زیرِ", "سابق", "ساخته", "سازي", "سالانه", "سالیانه", "سایر", "سراسر", "سرانجام", "سریعا", "سریِ", "سعي", "سمتِ", "سوم", "سوي", "سوی", "سویِ", "سپس", "شان", "شايد", "شاید", "شخصا", "شد", "شدم", "شدن", "شدند", "شده", "شدی", "شدید", "شدیدا", "شدیم", "شش", "شش  نداشته", "شما", "شناسي", "شود", "شوم", "شوند", "شونده", "شوی", "شوید", "شویم", "صرفا", "صورت", "ضدِّ", "ضدِّ", "ضمن", "طبعا", "طبقِ", "طبیعتا", "طرف", "طريق", "طریق", "طور", "طي", "طی", "ظاهرا", "عدم", "عقبِ", "علّتِ", "علیه", "عمدا", "عمدتا", "عمل", "عملا", "عنوان", "عنوانِ", "غالبا", "غير", "غیر", "فردا", "فعلا", "فقط", "فكر", "فوق", "قابل", "قبل", "قبلا", "قدری", "قصدِ", "قطعا", "كرد", "كردم", "كردن", "كردند", "كرده", "كسي", "كل", "كمتر", "كند", "كنم", "كنند", "كنيد", "كنيم", "كه", "لااقل", "لطفا", "لطفاً", "ما", "مان", "مانند", "مانندِ", "مبادا", "متاسفانه", "متعاقبا", "مثل", "مثلا", "مثلِ", "مجانی", "مجددا", "مجموعا", "مختلف", "مدام", "مدت", "مدّتی", "مردم", "مرسی", "مستقیما", "مسلما", "مطمینا", "معمولا", "مقابل", "ممکن", "من", "موارد", "مورد", "موقتا", "مي", "ميليارد", "ميليون", "مگر", "می", "می شود", "میان", "می‌رسد", "می‌رود", "می‌شود", "می‌کنیم", "ناشي", "نام", "ناگاه", "ناگهان", "ناگهانی", "نبايد", "نباید", "نبود", "نخست", "نخستين", "نخواهد", "نخواهم", "نخواهند", "نخواهی", "نخواهید", "نخواهیم", "ندارد", "ندارم", "ندارند", "نداری", "ندارید", "نداریم", "نداشت", "نداشتم", "نداشتند", "نداشته", "نداشتی", "نداشتید", "نداشتیم", "نزديك", "نزدِ", "نزدیکِ", "نسبتا", "نشان", "نشده", "نظير", "نظیر", "نكرده", "نمايد", "نمي", "نمی", "نمی‌شود", "نه", "نهایتا", "نوع", "نوعي", "نوعی", "نيز", "نيست", "نگاه", "نیز", "نیست", "ها", "هاي", "هايي", "های", "هایی", "هبچ", "هر", "هرچه", "هرگز", "هزار", "هست", "هستم", "هستند", "هستيم", "هستی", "هستید", "هستیم", "هفت", "هم", "همان", "همه", "همواره", "همين", "همچنان", "همچنين", "همچنین", "همچون", "همیشه", "همین", "هنوز", "هنگام", "هنگامِ", "هنگامی", "هيچ", "هیچ", "هیچگاه", "و", "واقعا", "واقعی", "وجود", "وسطِ", "وضع", "وقتي", "وقتی", "وقتیکه", "ولی", "وي", "وگو", "وی", "ویژه", "يا", "يابد", "يك", "يكديگر", "يكي", "ّه", "٪", "پارسال", "پاعینِ", "پس", "پنج", "پيش", "پیدا", "پیش", "پیشاپیش", "پیشتر", "پیشِ", "چرا", "چطور", "چقدر", "چنان", "چنانچه", "چنانکه", "چند", "چندین", "چنين", "چنین", "چه", "چهار", "چو", "چون", "چيزي", "چگونه", "چیز", "چیزی", "چیست", "کاش", "کامل", "کاملا", "کتبا", "کجا", "کجاست", "کدام", "کرد", "کردم", "کردن", "کردند", "کرده", "کردی", "کردید", "کردیم", "کس", "کسانی", "کسی", "کل", "کلا", "کم", "کماکان", "کمتر", "کمتری", "کمی", "کن", "کنار", "کنارِ", "کند", "کنم", "کنند", "کننده", "کنون", "کنونی", "کنی", "کنید", "کنیم", "که", "کو", "کَی", "کی", "گاه", "گاهی", "گذاري", "گذاشته", "گذشته", "گردد", "گرفت", "گرفتم", "گرفتن", "گرفتند", "گرفته", "گرفتی", "گرفتید", "گرفتیم", "گروهي", "گفت", "گفتم", "گفتن", "گفتند", "گفته", "گفتی", "گفتید", "گفتیم", "گه", "گهگاه", "گو", "گويد", "گويند", "گویا", "گوید", "گویم", "گویند", "گویی", "گویید", "گوییم", "گيرد", "گيري", "گیرد", "گیرم", "گیرند", "گیری", "گیرید", "گیریم", "ی", "یا", "یابد", "یابم", "یابند", "یابی", "یابید", "یابیم", "یافت", "یافتم", "یافتن", "یافته", "یافتی", "یافتید", "یافتیم", "یعنی", "یقینا", "یه", "یک", "یکی", "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        }
    }

}
