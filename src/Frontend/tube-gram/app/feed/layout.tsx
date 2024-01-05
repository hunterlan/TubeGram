export default function FeedLayout({children,}: {
    children: React.ReactNode
}) {
    return (
        <>
            <section className='grid sm:grid-cols-1 md:grid-cols-2 p-4 lg:px-6'>{children}</section>
        </>
    )
}