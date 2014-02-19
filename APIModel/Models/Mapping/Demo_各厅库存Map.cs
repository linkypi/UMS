using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_各厅库存Map : EntityTypeConfiguration<Demo_各厅库存>
    {
        public Demo_各厅库存Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.制式)
                .HasMaxLength(50);

            this.Property(t => t.型号)
                .HasMaxLength(50);

            this.Property(t => t.价钱)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_各厅库存");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.制式).HasColumnName("制式");
            this.Property(t => t.型号).HasColumnName("型号");
            this.Property(t => t.价钱).HasColumnName("价钱");
            this.Property(t => t.东区).HasColumnName("东区");
            this.Property(t => t.中山港).HasColumnName("中山港");
            this.Property(t => t.东凤).HasColumnName("东凤");
            this.Property(t => t.东凤新华厅).HasColumnName("东凤新华厅");
            this.Property(t => t.港口).HasColumnName("港口");
            this.Property(t => t.西区).HasColumnName("西区");
            this.Property(t => t.横栏).HasColumnName("横栏");
            this.Property(t => t.沙朗).HasColumnName("沙朗");
            this.Property(t => t.坦背).HasColumnName("坦背");
            this.Property(t => t.大涌).HasColumnName("大涌");
            this.Property(t => t.江门育德店).HasColumnName("江门育德店");
            this.Property(t => t.江门地王).HasColumnName("江门地王");
            this.Property(t => t.海州卖场).HasColumnName("海州卖场");
            this.Property(t => t.大涌华泰店).HasColumnName("大涌华泰店");
            this.Property(t => t.东升店).HasColumnName("东升店");
            this.Property(t => t.恒信).HasColumnName("恒信");
            this.Property(t => t.南头同乐店).HasColumnName("南头同乐店");
            this.Property(t => t.大信中庭).HasColumnName("大信中庭");
            this.Property(t => t.板芙).HasColumnName("板芙");
            this.Property(t => t.明珠).HasColumnName("明珠");
            this.Property(t => t.横栏四沙).HasColumnName("横栏四沙");
            this.Property(t => t.三乡文昌).HasColumnName("三乡文昌");
            this.Property(t => t.黄圃新明).HasColumnName("黄圃新明");
            this.Property(t => t.同安).HasColumnName("同安");
            this.Property(t => t.濠头).HasColumnName("濠头");
            this.Property(t => t.沙溪云汉营业厅).HasColumnName("沙溪云汉营业厅");
            this.Property(t => t.沙溪宝珠店).HasColumnName("沙溪宝珠店");
            this.Property(t => t.火炬职院).HasColumnName("火炬职院");
            this.Property(t => t.三乡天翼3G体验馆).HasColumnName("三乡天翼3G体验馆");
            this.Property(t => t.三乡通大店).HasColumnName("三乡通大店");
            this.Property(t => t.小榄泰丰).HasColumnName("小榄泰丰");
            this.Property(t => t.城区政企).HasColumnName("城区政企");
            this.Property(t => t.综合部).HasColumnName("综合部");
            this.Property(t => t.中捷政企仓).HasColumnName("中捷政企仓");
            this.Property(t => t.呼叫).HasColumnName("呼叫");
            this.Property(t => t.海州正联).HasColumnName("海州正联");
            this.Property(t => t.南朗店).HasColumnName("南朗店");
            this.Property(t => t.员工购机).HasColumnName("员工购机");
            this.Property(t => t.黄圃).HasColumnName("黄圃");
            this.Property(t => t.FTTH).HasColumnName("FTTH");
            this.Property(t => t.大信中庭旧).HasColumnName("大信中庭旧");
            this.Property(t => t.沙溪).HasColumnName("沙溪");
            this.Property(t => t.思致).HasColumnName("思致");
            this.Property(t => t.中山港1_1).HasColumnName("中山港1+1");
            this.Property(t => t.批发仓).HasColumnName("批发仓");
            this.Property(t => t.卡批发仓).HasColumnName("卡批发仓");
            this.Property(t => t.城区校园促销).HasColumnName("城区校园促销");
            this.Property(t => t.黄圃校园促销).HasColumnName("黄圃校园促销");
            this.Property(t => t.恒信旧).HasColumnName("恒信旧");
            this.Property(t => t.南头).HasColumnName("南头");
            this.Property(t => t.悦来南).HasColumnName("悦来南");
            this.Property(t => t.二级分销).HasColumnName("二级分销");
            this.Property(t => t.天翼营销中心).HasColumnName("天翼营销中心");
            this.Property(t => t.三乡).HasColumnName("三乡");
            this.Property(t => t.民众).HasColumnName("民众");
            this.Property(t => t.总仓).HasColumnName("总仓");
            this.Property(t => t.翠亨).HasColumnName("翠亨");
            this.Property(t => t.南区).HasColumnName("南区");
            this.Property(t => t.五桂山).HasColumnName("五桂山");
            this.Property(t => t.张家边).HasColumnName("张家边");
            this.Property(t => t.升平).HasColumnName("升平");
            this.Property(t => t.东升).HasColumnName("东升");
            this.Property(t => t.天讯达_新_).HasColumnName("天讯达(新)");
            this.Property(t => t.维修室).HasColumnName("维修室");
            this.Property(t => t.天讯达).HasColumnName("天讯达");
            this.Property(t => t.桃园名店).HasColumnName("桃园名店");
            this.Property(t => t.小榄泰丰旧).HasColumnName("小榄泰丰旧");
            this.Property(t => t.五桂山校园).HasColumnName("五桂山校园");
            this.Property(t => t.三角).HasColumnName("三角");
            this.Property(t => t.白石).HasColumnName("白石");
            this.Property(t => t.赠品仓).HasColumnName("赠品仓");
            this.Property(t => t.神湾).HasColumnName("神湾");
            this.Property(t => t.新城).HasColumnName("新城");
            this.Property(t => t.坦洲).HasColumnName("坦洲");
            this.Property(t => t.南朗).HasColumnName("南朗");
            this.Property(t => t.阜沙).HasColumnName("阜沙");
            this.Property(t => t.坦洲申堂店).HasColumnName("坦洲申堂店");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
