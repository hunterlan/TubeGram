import Image from "next/image";
import Link from "next/link";

export default async function Home() {
  return (
    <main>
      <div className='flex'>
        <Image src="/table_photos.png" alt="Table with photos" width="1024" height="1024"/>
        <div>
          <h1>Simple feed with photos and videos</h1>
          <div>
            <Link href="/login"><button>Sign in</button></Link>
            <Link href="/signup"><button>Sign up</button></Link>
          </div>
        </div>
      </div>
    </main>
  )
}
