//
// Terminaux  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using Terminaux.Inputs.Interactive;

namespace Terminaux.Console.Fixtures.Cases.CaseData
{
    internal class CliInfoPaneRtlTestData : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        internal static Dictionary<string, string> strings = new()
        {
            {"الفقرة الأولى", "تم إطلاق Nitrocid KS (محاكي kernel آنذاك) في عام 2018 بالإصدار 0.0.1، والذي كان لا يزال في مرحلة الوصول المبكر (ألفا). يحتوي على محاكي أساسي للغاية يركز فقط على الأشياء المتوفرة: النواة وآلية تسجيل الدخول وتطبيق الصدفة. منذ ذلك الحين، قمنا بإجراء العديد من التحسينات، وبلغت ذروتها في المرحلة \"التجريبية\" للنواة، الإصدار 0.1.0. لا تعمل النواة بشكل مستقل فحسب، بل يمكن أيضًا تشغيلها باستخدام GRILO، مما يجعلها محاكي كمبيوتر متكامل. GRILO هو محاكي أداة تحميل التمهيد الذي يسمح لك بجعل تطبيقات C# وVisual Basic قابلة للتمهيد من أداة تحميل التمهيد التي تمت محاكاتها."},
            {"الفقرة الثانية", "بعد ترقية Nitrocid KS 0.0.24.x إلى 0.1.0، قد يؤدي تشغيله إلى ظهور معالج التشغيل الأول معتقدًا أنه سيتم فقدان كل التكوينات الخاصة بك. ومع ذلك، لم يتم فقدانها، حيث لن تتم إزالة أنماط التكوين القديمة بحلول 0.1.0. لسوء الحظ، أسلوبا التكوين هذين غير متوافقين مع بعضهما البعض، مما يعني أنه يجب عليك نسخ التكوين الذي قمت به يدويًا عند استخدام 0.0.24.x. يتم ذلك عن عمد لجعل قراء وكتابة التكوين أسرع من ذي قبل باستخدام تقنية التسلسل مقارنة بالطريقة القديمة المتمثلة في تحليل كل مفتاح تكوين يدويًا وتثبيته في المتغير المناسب. يوفر القراء والكتاب الجدد أيضًا مرونة أكبر في طريقة تنفيذها، مما يجعل تعديلاتك أكثر قابلية للتكوين من ذي قبل."},
            {"الفقرة الثالثة", "لقد قمنا بإزالة استخدام أجزاء التعديل لتنظيم تعديلات متعددة بنفس الاسم لأنها تكون مرتبطة عندما يتكون التعديل من ملف كود مصدر واحد فقط (Visual Basic أو C#) وسيتم تجميع التعديلات الكبيرة باستخدام ملفات كود مصدر متعددة، ملفات التعليمات البرمجية المصدر هذه متصلة ببعضها البعض. تعمل على بعضها البعض. منذ بعض الإصدارات، قمنا بإزالة التعديلات المستندة إلى التعليمات البرمجية المصدر واعتمدنا على مطوري التعديلات لإنشاء إصدارات .DLL لضمان حصولهم على أقصى قدر من المرونة. ونظرًا لصلابتها، لن تظهر هذه الميزة بعد الآن. جميع إصدارات واجهة برمجة التطبيقات Nitrocid KS قبل الإصدار 3.0 (بما في ذلك 0.0.24.x) كانت تحتوي على KS كمساحة اسم جذر لها، حيث كان الاسم في ذلك الوقت هو Kernel Simulator. قمنا بتسمية التطبيق Nitrocid KS وتم تغيير اسم النواة إلى Nitrocid Kernel. ولذلك، تم تغيير مساحة الاسم الجذر من KS إلى Nitrocid لتكون أكثر اتساقًا مع العلامة التجارية للتطبيق. لأسباب تاريخية، لا تزال بعض تعليقات التغيير المعطلة تستخدم مساحة اسم جذر KS ولا ينبغي تعديلها لأنها تسرد تاريخ التغييرات التي حدثت بين الإصدار السابق والإصدار التالي. يجب عليك تحديث عمليات الاستيراد للإشارة إلى مساحة الاسم الجذرية الجديدة."},
        };

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            strings.Keys;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item) =>
            strings[item];

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;
    }
}
