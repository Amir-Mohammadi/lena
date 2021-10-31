using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Post : IEntity
  {
    protected internal Post()
    {
      this.ChildPosts = new HashSet<Post>();
      this.UserPosts = new HashSet<UserPost>();
      this.PostMessageSends = new HashSet<PostMessageSend>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Nullable<int> PostId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Post ParentPost { get; set; }
    public virtual ICollection<Post> ChildPosts { get; set; }
    public virtual ICollection<UserPost> UserPosts { get; set; }
    public virtual ICollection<PostMessageSend> PostMessageSends { get; set; }
  }
}
