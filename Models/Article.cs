using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace razor09_razorweb.models{
    public class Article{
        [Key]
        public int Id { get; set; }
        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} phải dài từ {2} đến {1} kí tự")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [Column(TypeName = "nvarchar")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Phải nhập {0}")]
        [DisplayName("Ngày tạo")]
        public DateTime Created { get; set; }
        [Column(TypeName = "ntext")]
        [DisplayName("Nội dung")]
        [Required(ErrorMessage = "Phải nhập {0}")]
        public string Content { get; set; }
    }
}